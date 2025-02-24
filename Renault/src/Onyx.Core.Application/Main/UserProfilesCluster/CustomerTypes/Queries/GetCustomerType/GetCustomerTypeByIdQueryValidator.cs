using FluentValidation;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;
public class GetCustomerTypeByIdQueryValidator : AbstractValidator<GetCustomerTypeByIdQuery>
{
    public GetCustomerTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}