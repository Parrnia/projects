using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.UpdateProductAttributeOptionValue;
public class UpdateProductAttributeOptionValueCommandValidator : AbstractValidator<UpdateProductAttributeOptionValueCommand>
{
    public UpdateProductAttributeOptionValueCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
    }
}