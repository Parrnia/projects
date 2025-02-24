using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.UpdateReturnOrderItem;
public class UpdateReturnOrderItemCommandValidator : AbstractValidator<UpdateReturnOrderItemCommand>
{
    public UpdateReturnOrderItemCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Quantity)
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات دلیل اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازگشت اجباریست");
    }
}
