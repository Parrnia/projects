using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmSomeReturnOrder;
public class ConfirmSomeReturnOrderCommandValidator : AbstractValidator<ConfirmSomeReturnOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public ConfirmSomeReturnOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
