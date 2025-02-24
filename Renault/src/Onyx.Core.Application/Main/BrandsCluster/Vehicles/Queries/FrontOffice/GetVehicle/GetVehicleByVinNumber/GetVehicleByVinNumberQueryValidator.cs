using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicle.GetVehicleByVinNumber;
public class GetVehicleByVinNumberQueryValidator : AbstractValidator<GetVehicleByVinNumberQuery>
{
    public GetVehicleByVinNumberQueryValidator()
    {
        RuleFor(x => x.VinNumber)
            .NotEmpty().WithMessage("شماره وین اجباریست");
    }
}