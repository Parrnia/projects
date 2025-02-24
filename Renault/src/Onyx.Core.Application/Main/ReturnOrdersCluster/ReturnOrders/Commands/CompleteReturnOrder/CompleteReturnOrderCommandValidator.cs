using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CompleteReturnOrder;
public class CompleteReturnOrderCommandValidator : AbstractValidator<CompleteReturnOrderCommand>
{
    public CompleteReturnOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
    }
}
