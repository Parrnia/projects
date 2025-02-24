using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetVehicleBrandsForBlock;
public class GetVehicleBrandsForBlockQueryValidator : AbstractValidator<GetVehicleBrandsForBlockQuery>
{
    public GetVehicleBrandsForBlockQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1).WithMessage("محدوده باید بزرگتر یا مساوی یک باشد");
    }
}