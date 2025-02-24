using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.CancelOrder;
public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public CancelOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;



        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست"); 
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
