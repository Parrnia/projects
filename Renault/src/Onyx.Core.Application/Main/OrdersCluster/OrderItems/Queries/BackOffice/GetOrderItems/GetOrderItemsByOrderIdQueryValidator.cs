using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice.GetOrderItems;
public class GetOrderItemsByOrderIdQueryValidator : AbstractValidator<GetOrderItemsByOrderIdQuery>
{
    public GetOrderItemsByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
