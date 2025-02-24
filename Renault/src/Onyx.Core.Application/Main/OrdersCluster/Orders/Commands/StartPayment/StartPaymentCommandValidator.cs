using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.StartPayment;
public class StartPaymentCommandValidator : AbstractValidator<StartPaymentCommand>
{
    private readonly IApplicationDbContext _context;
    public StartPaymentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
