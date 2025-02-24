using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;
public class GetOrderStatesByOrderIdQueryValidator : AbstractValidator<GetOrderStatesByOrderIdQuery>
{
    public GetOrderStatesByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
