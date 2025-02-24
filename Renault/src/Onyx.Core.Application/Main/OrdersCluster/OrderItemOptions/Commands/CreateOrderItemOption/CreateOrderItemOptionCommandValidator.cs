using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Commands.CreateOrderItemOption;
public class CreateOrderItemOptionCommandValidator : AbstractValidator<CreateOrderItemOptionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderItemOptionCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.OrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش اجباریست");
    }
}
