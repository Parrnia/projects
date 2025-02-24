using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterialCustomFields.Commands.CreateProductOptionMaterialCustomField;
public class CreateProductOptionMaterialCustomFieldCommandValidator : AbstractValidator<CreateProductOptionMaterialCustomFieldCommand>
{
    public CreateProductOptionMaterialCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductOptionMaterialId)
            .NotEmpty().WithMessage("شناسه آپشن جنس محصول اجباریست");
    }
}
