using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.ShipOrder;
public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public ShipOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
