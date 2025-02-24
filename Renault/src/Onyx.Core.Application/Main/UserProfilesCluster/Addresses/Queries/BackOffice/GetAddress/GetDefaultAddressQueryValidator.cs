using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddress;
public class GetDefaultAddressQueryValidator : AbstractValidator<GetDefaultAddressQuery>
{
    public GetDefaultAddressQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}