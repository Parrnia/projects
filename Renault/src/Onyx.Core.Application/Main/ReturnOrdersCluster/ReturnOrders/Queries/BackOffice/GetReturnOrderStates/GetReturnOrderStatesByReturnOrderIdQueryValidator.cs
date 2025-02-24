using FluentValidation;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrderStates;
public class GetOrderStatesByOrderIdQueryValidator : AbstractValidator<GetOrderStatesByOrderIdQuery>
{
    public GetOrderStatesByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
