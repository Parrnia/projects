using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;
public class DeleteRangeReturnOrderItemCommandValidator : AbstractValidator<DeleteRangeReturnOrderItemCommand>
{
    public DeleteRangeReturnOrderItemCommandValidator()
    {
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم های سفارش بازشگت مرتبط اجباریست");
    }
}
