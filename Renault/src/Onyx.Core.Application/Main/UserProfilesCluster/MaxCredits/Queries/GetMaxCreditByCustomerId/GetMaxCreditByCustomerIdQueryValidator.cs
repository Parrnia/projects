using FluentValidation;
using Onyx.Application.Helpers;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddress.GetAddressById;

namespace Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditByCustomerId;
public class GetMaxCreditByCustomerIdQueryValidator : AbstractValidator<GetMaxCreditByCustomerIdQuery>
{
    public GetMaxCreditByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}