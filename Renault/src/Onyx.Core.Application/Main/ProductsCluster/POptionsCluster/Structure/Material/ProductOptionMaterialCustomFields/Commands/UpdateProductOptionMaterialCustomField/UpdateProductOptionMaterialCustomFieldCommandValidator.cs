using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterialCustomFields.Commands.UpdateProductOptionMaterialCustomField;
public class UpdateProductOptionMaterialCustomFieldCommandValidator : AbstractValidator<UpdateProductOptionMaterialCustomFieldCommand>
{
    public UpdateProductOptionMaterialCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductOptionMaterialId)
            .NotEmpty().WithMessage("شناسه آپشن جنس محصول اجباریست");
    }
}