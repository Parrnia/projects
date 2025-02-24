using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.CreateProductAttributeOptionValue;
public class CreateProductAttributeOptionValueCommandValidator : AbstractValidator<CreateProductAttributeOptionValueCommand>
{
    public CreateProductAttributeOptionValueCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
    }
}
