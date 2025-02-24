using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.AcceptReturnOrder;
public class AcceptReturnOrderCommandValidator : AbstractValidator<AcceptReturnOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public AcceptReturnOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
