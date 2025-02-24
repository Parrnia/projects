using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByKindId;
public class GetVehiclesByKindIdQueryValidator : AbstractValidator<GetVehiclesByKindIdQuery>
{
    public GetVehiclesByKindIdQueryValidator()
    {
        RuleFor(x => x.KindId)
            .NotEmpty().WithMessage("شناسه نوع اجباریست");
    }
}
