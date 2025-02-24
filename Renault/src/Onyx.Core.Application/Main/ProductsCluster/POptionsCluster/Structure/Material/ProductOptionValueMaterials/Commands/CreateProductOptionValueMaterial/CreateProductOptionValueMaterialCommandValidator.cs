using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.CreateProductOptionValueMaterial;
public class CreateProductOptionValueMaterialCommandValidator : AbstractValidator<CreateProductOptionValueMaterialCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public CreateProductOptionValueMaterialCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ProductOptionMaterialId)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه آپشن جنس محصول اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MustAsync(BeUniqueName).WithMessage("یکی از محصولات، یک ویژگی محصول با این نام دارد")
            .MustAsync(BeUniqueNameForOption).WithMessage("یک آپشن نمیتواند ویژگی محصول با نام تکراری داشته باشد");
    }

    public async Task<bool> BeUniqueNameForOption(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionMaterials
            .Include(c => c.Values)
            .SingleOrDefaultAsync(c => c.Id == _id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionValueColor), _id);
        }


        if (entity.Values.Any(c => c.Name == name))
        {
            return false;
        }


        return true;
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .Where(c => c.MaterialOptionId == _id)
            .Include(c => c.Attributes);

        foreach (var product in products)
        {
            if (product.Attributes.Any(c => c.Name == name))
            {
                return false;
            }
        }

        return true;
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
