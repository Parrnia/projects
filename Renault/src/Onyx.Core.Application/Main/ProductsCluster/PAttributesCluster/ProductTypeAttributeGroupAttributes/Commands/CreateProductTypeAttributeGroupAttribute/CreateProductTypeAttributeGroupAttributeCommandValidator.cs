using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.CreateProductTypeAttributeGroupAttribute;
public class CreateProductTypeAttributeGroupAttributeCommandValidator : AbstractValidator<CreateProductTypeAttributeGroupAttributeCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public CreateProductTypeAttributeGroupAttributeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.ProductTypeAttributeGroupId)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه گروه بندی نوع ویژگی محصول اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست")
            .MustAsync(BeUniqueName).WithMessage("یکی از محصولات، یک ویژگی محصول با این نام دارد");
    }

    public async Task<bool> BeUniqueName(string value, CancellationToken cancellationToken)
    {
        var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
            .Include(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
        .FirstOrDefaultAsync(x => x.Id == _id, cancellationToken);

        if (productTypeAttributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), _id);
        }


        foreach (var product in productTypeAttributeGroup.ProductAttributeTypes.SelectMany(c => c.Products))
        {
            if (product.Attributes.Any(c => c.Name == value))
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
