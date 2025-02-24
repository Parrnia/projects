using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotal.GetSendReturnOrderTotal;
public class GetSendReturnOrderTotalByReturnOrderIdQueryValidator : AbstractValidator<GetSendReturnOrderTotalByReturnOrderIdQuery>
{
    public GetSendReturnOrderTotalByReturnOrderIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه گروه آیتم سفارش بازگشت اجباریست");
    }
}
