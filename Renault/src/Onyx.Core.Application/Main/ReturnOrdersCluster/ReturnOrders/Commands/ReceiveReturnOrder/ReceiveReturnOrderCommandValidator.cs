using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReceiveReturnOrder;
public class ReceiveReturnOrderCommandValidator : AbstractValidator<ReceiveReturnOrderCommand>
{
    public ReceiveReturnOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
