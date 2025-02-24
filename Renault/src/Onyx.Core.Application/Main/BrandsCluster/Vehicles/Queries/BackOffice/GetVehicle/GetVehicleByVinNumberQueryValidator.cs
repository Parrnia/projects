using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicle;
public class GetVehicleByVinNumberQueryValidator : AbstractValidator<GetVehicleByVinNumberQuery>
{
    public GetVehicleByVinNumberQueryValidator()
    {
        RuleFor(x => x.VinNumber)
            .NotEmpty().WithMessage("شماره وین اجباریست");
    }
}