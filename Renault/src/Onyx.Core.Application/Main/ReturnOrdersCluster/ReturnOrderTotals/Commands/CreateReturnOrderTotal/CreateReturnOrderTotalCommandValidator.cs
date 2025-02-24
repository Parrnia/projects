using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.CreateReturnOrderTotal;
public class CreateReturnOrderTotalCommandValidator : AbstractValidator<CreateReturnOrderTotalCommand>
{
    public CreateReturnOrderTotalCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Price)
            .NotEmpty().WithMessage("مبلغ اجباریست");
        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("نوع اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderTotalApplyType)
            .IsInEnum().WithMessage("نوع اجباریست");
    }
}
