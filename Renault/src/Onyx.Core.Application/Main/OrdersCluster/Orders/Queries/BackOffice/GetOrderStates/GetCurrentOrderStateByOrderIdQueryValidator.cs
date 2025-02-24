using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;
public class GetCurrentOrderStateByOrderIdQueryValidator : AbstractValidator<GetCurrentOrderStateByOrderIdQuery>
{
    public GetCurrentOrderStateByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
