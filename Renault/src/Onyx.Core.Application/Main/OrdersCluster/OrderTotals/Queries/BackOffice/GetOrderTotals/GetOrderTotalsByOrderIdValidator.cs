using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice.GetOrderTotals;
public class GetOrderTotalsByOrderIdQueryValidator : AbstractValidator<GetOrderTotalsByOrderIdQuery>
{
    public GetOrderTotalsByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
