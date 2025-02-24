using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Queries.FrontOffice.GetUser;
public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");

    }
}