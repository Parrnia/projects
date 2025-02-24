using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.UpdateProductOptionValueMaterial;
public class UpdateProductOptionValueMaterialCommandValidator : AbstractValidator<UpdateProductOptionValueMaterialCommand>
{
    public UpdateProductOptionValueMaterialCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.ProductOptionMaterialId)
            .NotEmpty().WithMessage("شناسه آپشن جنس محصول اجباریست");
    }
}