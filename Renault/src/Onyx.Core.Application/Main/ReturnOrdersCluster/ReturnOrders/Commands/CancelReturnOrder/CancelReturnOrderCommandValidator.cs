using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CancelReturnOrder;
public class CancelReturnOrderCommandValidator : AbstractValidator<CancelReturnOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public CancelReturnOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
