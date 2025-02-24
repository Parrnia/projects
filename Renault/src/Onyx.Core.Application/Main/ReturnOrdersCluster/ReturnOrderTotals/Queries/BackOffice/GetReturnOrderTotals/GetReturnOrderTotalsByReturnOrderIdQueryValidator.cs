using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotals;
public class GetReturnOrderTotalsByReturnOrderIdQueryValidator : AbstractValidator<GetReturnOrderTotalsByReturnOrderIdQuery>
{
    public GetReturnOrderTotalsByReturnOrderIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه گروه آیتم سفارش بازگشت اجباریست");
    }
}
