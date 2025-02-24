using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.RemoveVehicleFromCustomer;
public class RemoveVehicleFromCustomerCommandValidator : AbstractValidator<RemoveVehicleFromCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    public RemoveVehicleFromCustomerCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");

        RuleFor(v => v.VehicleId)
            .NotEmpty().WithMessage("شناسه خودرو اجباریست");
    }
}