using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice.GetCustomer;
public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}