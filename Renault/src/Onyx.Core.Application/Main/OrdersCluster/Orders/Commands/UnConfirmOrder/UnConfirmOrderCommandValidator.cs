using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UnConfirmOrder;
public class UnConfirmOrderCommandValidator : AbstractValidator<UnConfirmOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public UnConfirmOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
