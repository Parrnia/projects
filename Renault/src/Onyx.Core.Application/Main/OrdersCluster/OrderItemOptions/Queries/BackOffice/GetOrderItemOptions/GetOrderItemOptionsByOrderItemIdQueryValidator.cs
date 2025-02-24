using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.BackOffice.GetOrderItemOptions;
public class GetOrderItemOptionsByOrderItemIdQueryValidator : AbstractValidator<GetOrderItemOptionsByOrderItemIdQuery>
{
    public GetOrderItemOptionsByOrderItemIdQueryValidator()
    {
        RuleFor(x => x.OrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش اجباریست");
    }
}
