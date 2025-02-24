using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice.GetOptionValues;
public class GetOptionValuesByReturnOrderItemGroupIdQueryValidator : AbstractValidator<GetOptionValuesByReturnOrderItemGroupIdQuery>
{
    public GetOptionValuesByReturnOrderItemGroupIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم سفارش بازگشت اجباریست");
    }
}
