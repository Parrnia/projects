using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PrepareOrder;
public class PrepareOrderCommandValidator : AbstractValidator<PrepareOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public PrepareOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
