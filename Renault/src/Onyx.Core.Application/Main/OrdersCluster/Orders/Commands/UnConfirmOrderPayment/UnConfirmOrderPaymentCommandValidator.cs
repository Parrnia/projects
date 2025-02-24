using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UnConfirmOrderPayment;
public class UnConfirmOrderPaymentCommandValidator : AbstractValidator<UnConfirmOrderPaymentCommand>
{
    private readonly IApplicationDbContext _context;
    public UnConfirmOrderPaymentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
