using FluentValidation;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;
public class GetCustomerTypeByCustomerTypeEnumQueryValidator : AbstractValidator<GetCustomerTypeByCustomerTypeEnumQuery>
{
    public GetCustomerTypeByCustomerTypeEnumQueryValidator()
    {
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}