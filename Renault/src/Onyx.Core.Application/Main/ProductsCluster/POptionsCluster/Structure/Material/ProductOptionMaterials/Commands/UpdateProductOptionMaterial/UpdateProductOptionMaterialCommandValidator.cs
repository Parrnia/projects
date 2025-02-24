using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.UpdateProductOptionMaterial;
public class UpdateProductOptionMaterialCommandValidator : AbstractValidator<UpdateProductOptionMaterialCommand>
{
    public UpdateProductOptionMaterialCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
    }
}