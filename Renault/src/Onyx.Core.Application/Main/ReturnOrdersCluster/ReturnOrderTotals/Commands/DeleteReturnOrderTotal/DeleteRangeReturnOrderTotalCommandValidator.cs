using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;
public class DeleteRangeReturnOrderTotalCommandValidator : AbstractValidator<DeleteRangeReturnOrderTotalCommand>
{
    public DeleteRangeReturnOrderTotalCommandValidator()
    {
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
    }
}
