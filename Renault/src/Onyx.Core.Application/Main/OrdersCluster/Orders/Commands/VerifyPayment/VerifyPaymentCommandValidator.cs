using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.VerifyPayment;
public class VerifyPaymentCommandValidator : AbstractValidator<VerifyPaymentCommand>
{
    private readonly IApplicationDbContext _context;
    public VerifyPaymentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.PaymentId)
            .NotEmpty().WithMessage("شناسه پرداخت سفارش اجباریست");
    }
}
