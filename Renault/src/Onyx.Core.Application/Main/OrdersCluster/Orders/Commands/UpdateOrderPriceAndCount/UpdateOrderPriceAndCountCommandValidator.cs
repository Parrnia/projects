using FluentValidation;
using Onyx.Application.Helpers;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrderPriceAndCount;

public class UpdateOrderPriceAndCountCommandValidator : AbstractValidator<UpdateOrderPriceAndCountCommand>
{
    public UpdateOrderPriceAndCountCommandValidator()
    {

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}

