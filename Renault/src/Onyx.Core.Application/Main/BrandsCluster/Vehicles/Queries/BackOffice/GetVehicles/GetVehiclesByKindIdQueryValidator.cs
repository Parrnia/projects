using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicles;
public class GetVehiclesByKindIdQueryValidator : AbstractValidator<GetVehiclesByKindIdQuery>
{
    public GetVehiclesByKindIdQueryValidator()
    {
        RuleFor(x => x.KindId)
            .NotEmpty().WithMessage("شناسه نوع اجباریست");
    }
}
