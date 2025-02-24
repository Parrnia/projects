using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.CreateProductAttributeType;
public class CreateProductAttributeTypeCommandValidator : AbstractValidator<CreateProductAttributeTypeCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateProductAttributeTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست")
            .MustAsync(BeUniqueName).WithMessage("نوع ویژگی محصولی با این نام موجود است");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeTypes
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
