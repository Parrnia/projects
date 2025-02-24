using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddresses;
public class GetAddressesByCustomerIdQueryValidator : AbstractValidator<GetAddressesByCustomerIdQuery>
{
    public GetAddressesByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}