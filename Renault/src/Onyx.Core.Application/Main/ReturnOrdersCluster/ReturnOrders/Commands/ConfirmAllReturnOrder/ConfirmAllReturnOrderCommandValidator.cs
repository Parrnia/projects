using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmAllReturnOrder;
public class ConfirmAllReturnOrderCommandValidator : AbstractValidator<ConfirmAllReturnOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public ConfirmAllReturnOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
