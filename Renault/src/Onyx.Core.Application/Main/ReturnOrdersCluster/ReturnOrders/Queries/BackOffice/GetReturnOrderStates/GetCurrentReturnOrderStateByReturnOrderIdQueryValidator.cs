using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrderStates;
public class GetCurrentReturnOrderStateByReturnOrderIdQueryValidator : AbstractValidator<GetCurrentReturnOrderStateByReturnOrderIdQuery>
{
    public GetCurrentReturnOrderStateByReturnOrderIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازگشت اجباریست");
    }
}
