using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.UpdateReturnOrderTotal;
public class UpdateReturnOrderTotalCommandValidator : AbstractValidator<UpdateReturnOrderTotalCommand>
{
    public UpdateReturnOrderTotalCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
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
