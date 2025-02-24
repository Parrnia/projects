using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.RejectReturnOrder;
public class RejectReturnOrderCommandValidator : AbstractValidator<RejectReturnOrderCommand>
{
    public RejectReturnOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست"); 
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
