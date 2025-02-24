using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByCustomerId;
public class GetVehiclesByCustomerIdQueryValidator : AbstractValidator<GetVehiclesByCustomerIdQuery>
{
    public GetVehiclesByCustomerIdQueryValidator()
    {
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
