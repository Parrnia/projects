using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.CreateProductOptionMaterial;
public class CreateProductOptionMaterialCommandValidator : AbstractValidator<CreateProductOptionMaterialCommand>
{
    public CreateProductOptionMaterialCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
    }
}
