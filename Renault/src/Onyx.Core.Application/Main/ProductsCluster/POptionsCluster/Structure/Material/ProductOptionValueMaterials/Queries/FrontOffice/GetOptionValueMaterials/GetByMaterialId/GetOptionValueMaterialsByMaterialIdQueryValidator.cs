using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.FrontOffice.GetOptionValueMaterials.GetByMaterialId;
public class GetOptionValueMaterialsByMaterialIdQueryValidator : AbstractValidator<GetOptionValueMaterialsByMaterialIdQuery>
{
    public GetOptionValueMaterialsByMaterialIdQueryValidator()
    {
        RuleFor(x => x.MaterialId)
            .NotEmpty().WithMessage("شناسه جنس اجباریست");
    }
}