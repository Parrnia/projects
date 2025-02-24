using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeValueCustomFields.Commands.UpdateProductAttributeValueCustomField;
public class UpdateProductAttributeValueCustomFieldCommandValidator : AbstractValidator<UpdateProductAttributeValueCustomFieldCommand>
{
    public UpdateProductAttributeValueCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductAttributeId)
            .NotEmpty().WithMessage("شناسه مقدار ویژگی محصول اجباریست");
    }
}