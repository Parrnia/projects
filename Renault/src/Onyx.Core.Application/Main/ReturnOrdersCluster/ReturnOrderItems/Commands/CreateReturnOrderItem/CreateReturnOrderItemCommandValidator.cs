using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.CreateReturnOrderItem;
public class CreateReturnOrderItemCommandValidator : AbstractValidator<CreateReturnOrderItemCommand>
{
    public CreateReturnOrderItemCommandValidator()
    {
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
