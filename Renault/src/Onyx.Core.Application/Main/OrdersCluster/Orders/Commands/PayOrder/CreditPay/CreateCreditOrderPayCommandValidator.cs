using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;

public class CreateCreditOrderPayCommandValidator : AbstractValidator<CreateCreditOrderPayCommand>
{
    public CreateCreditOrderPayCommandValidator()
    {

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}

