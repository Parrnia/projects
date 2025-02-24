using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;
public class DeleteReturnOrderItemCommandValidator : AbstractValidator<DeleteReturnOrderItemCommand>
{
    public DeleteReturnOrderItemCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم های سفارش بازشگت مرتبط اجباریست");
    }
}
