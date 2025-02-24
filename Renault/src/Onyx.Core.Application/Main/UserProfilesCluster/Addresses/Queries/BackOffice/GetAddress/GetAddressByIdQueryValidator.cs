using FluentValidation;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddress;
public class GetAddressByIdQueryValidator : AbstractValidator<GetAddressByIdQuery>
{
    public GetAddressByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}