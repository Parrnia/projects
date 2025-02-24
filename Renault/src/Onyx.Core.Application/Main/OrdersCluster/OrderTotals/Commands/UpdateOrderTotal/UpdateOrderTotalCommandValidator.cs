using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Commands.UpdateOrderTotal;
public class UpdateOrderTotalCommandValidator : AbstractValidator<UpdateOrderTotalCommand>
{
    public UpdateOrderTotalCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Price)
            .NotEmpty().WithMessage("قیمت اجباریست");
        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("نوع اجباریست");
        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
