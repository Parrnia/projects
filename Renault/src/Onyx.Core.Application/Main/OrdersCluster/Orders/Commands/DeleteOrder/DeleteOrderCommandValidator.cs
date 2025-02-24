using FluentValidation;
using Onyx.Application.Helpers;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}

