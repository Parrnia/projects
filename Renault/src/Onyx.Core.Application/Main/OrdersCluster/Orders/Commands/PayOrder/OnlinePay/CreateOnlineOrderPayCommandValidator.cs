using FluentValidation;
using Onyx.Application.Helpers;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.OnlinePay;

public class CreateOnlineOrderPayCommandValidator : AbstractValidator<CreateOnlineOrderPayCommand>
{
    public CreateOnlineOrderPayCommandValidator()
    {
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
        RuleFor(v => v.PaymentService)
            .NotEmpty().WithMessage("نوع درگاه پرداخت اجباریست");
    }
}
