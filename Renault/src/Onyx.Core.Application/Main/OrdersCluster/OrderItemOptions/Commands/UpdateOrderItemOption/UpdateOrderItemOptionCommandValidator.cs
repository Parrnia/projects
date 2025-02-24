using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Commands.UpdateOrderItemOption;
public class UpdateOrderItemOptionCommandValidator : AbstractValidator<UpdateOrderItemOptionCommand>
{
    public UpdateOrderItemOptionCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.OrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش اجباریست");
    }

    
}
