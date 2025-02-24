using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SelfDeleteOrder;

public class SelfDeleteOrderCommandValidator : AbstractValidator<SelfDeleteOrderCommand>
{
    public SelfDeleteOrderCommandValidator()
    {

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}

