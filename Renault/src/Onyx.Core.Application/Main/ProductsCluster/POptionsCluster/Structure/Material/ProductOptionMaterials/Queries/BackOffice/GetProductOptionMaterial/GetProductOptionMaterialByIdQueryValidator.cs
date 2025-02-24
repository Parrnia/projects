using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterial;
public class GetProductOptionMaterialByIdQueryValidator : AbstractValidator<GetProductOptionMaterialByIdQuery>
{
    public GetProductOptionMaterialByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}