using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;
public class DeleteReturnOrderTotalCommandValidator : AbstractValidator<DeleteReturnOrderTotalCommand>
{
    public DeleteReturnOrderTotalCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
    }
}
