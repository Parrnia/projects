using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColorCustomFields.Commands.CreateProductOptionColorCustomField;
public class CreateProductOptionColorCustomFieldCommandValidator : AbstractValidator<CreateProductOptionColorCustomFieldCommand>
{

    public CreateProductOptionColorCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductOptionColorId)
            .NotEmpty().WithMessage("شناسه آپشن رنگ محصول اجباریست");
    }
}
