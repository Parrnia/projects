using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrand.GetVehicleBrandById;
public class GetVehicleBrandByIdQueryValidator : AbstractValidator<GetVehicleBrandByIdQuery>
{
    public GetVehicleBrandByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}