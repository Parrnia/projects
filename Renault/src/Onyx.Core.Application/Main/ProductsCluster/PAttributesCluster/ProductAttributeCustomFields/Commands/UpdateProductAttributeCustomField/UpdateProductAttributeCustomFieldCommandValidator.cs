using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeCustomFields.Commands.UpdateProductAttributeCustomField;
public class UpdateProductAttributeCustomFieldCommandValidator : AbstractValidator<UpdateProductAttributeCustomFieldCommand>
{
    public UpdateProductAttributeCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductAttributeId)
            .NotEmpty().WithMessage("شناسه ویژگی محصول اجباریست");
    }
}