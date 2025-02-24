using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditOnlinePay;

public class CreateCreditOnlineOrderPayCommandValidator : AbstractValidator<CreateCreditOnlineOrderPayCommand>
{
    public CreateCreditOnlineOrderPayCommandValidator()
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

