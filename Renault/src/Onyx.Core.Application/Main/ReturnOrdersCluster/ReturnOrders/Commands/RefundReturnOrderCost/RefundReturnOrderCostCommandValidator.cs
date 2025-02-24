using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.RefundReturnOrderCost;
public class RefundReturnOrderCostCommandValidator : AbstractValidator<RefundReturnOrderCostCommand>
{
    private readonly IApplicationDbContext _context;
    public RefundReturnOrderCostCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
        RuleFor(v => v.CostRefundType)
            .NotEmpty().WithMessage("شیوه بازپرداخت اجباریست");
    }
}
