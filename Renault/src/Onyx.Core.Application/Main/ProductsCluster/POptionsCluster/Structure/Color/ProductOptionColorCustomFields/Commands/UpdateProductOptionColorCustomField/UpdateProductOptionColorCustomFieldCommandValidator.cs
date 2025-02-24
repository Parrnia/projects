using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColorCustomFields.Commands.UpdateProductOptionColorCustomField;
public class UpdateProductOptionColorCustomFieldCommandValidator : AbstractValidator<UpdateProductOptionColorCustomFieldCommand>
{
    public UpdateProductOptionColorCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductOptionColorId)
            .NotEmpty().WithMessage("شناسه آپشن رنگ محصول اجباریست");
    }
}