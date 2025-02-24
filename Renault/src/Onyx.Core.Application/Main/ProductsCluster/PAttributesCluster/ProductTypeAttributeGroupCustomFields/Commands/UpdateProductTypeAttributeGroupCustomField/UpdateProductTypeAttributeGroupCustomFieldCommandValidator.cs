using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupCustomFields.Commands.UpdateProductTypeAttributeGroupCustomField;
public class UpdateProductTypeAttributeGroupCustomFieldCommandValidator : AbstractValidator<UpdateProductTypeAttributeGroupCustomFieldCommand>
{
    public UpdateProductTypeAttributeGroupCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductTypeAttributeGroupId)
            .NotEmpty().WithMessage("شناسه گروه بندی نوع ویژگی محصول اجباریست");
    }
}