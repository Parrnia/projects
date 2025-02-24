using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicle;
public class GetVehicleByIdQueryValidator : AbstractValidator<GetVehicleByIdQuery>
{
    public GetVehicleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}