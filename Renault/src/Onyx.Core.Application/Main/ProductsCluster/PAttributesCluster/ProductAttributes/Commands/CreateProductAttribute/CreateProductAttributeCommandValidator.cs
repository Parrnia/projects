using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.CreateProductAttribute;
public class CreateProductAttributeCommandValidator : AbstractValidator<CreateProductAttributeCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateProductAttributeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
        RuleFor(v => v.ValueName)
            .NotEmpty().WithMessage("مقدار اجباریست");
    }
}
