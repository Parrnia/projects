using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;

public class CreateReturnOrderCommandValidator : AbstractValidator<CreateReturnOrderCommand>
{
    private readonly IApplicationDbContext _context;


    public CreateReturnOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");

        RuleFor(v => v.ItemGroups.Select(c => c.ProductAttributeOptionId))
            .NotEmpty().WithMessage("شناسه ویژگی کالا اجباریست");
        RuleFor(v => v.Quantity)
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.Subtotal)
            .NotEmpty().WithMessage("جمع قیمت کل محصولات اجباریست");
        RuleFor(v => v.Total)
            .NotEmpty().WithMessage("مبلغ پرداختی اجباریست");

        RuleFor(v => v.ItemGroups.Select(c => c.OrderItems.Select(e => e.Quantity)))
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.ItemGroups.Select(c => c.OrderItems.Select(e => e.ReturnOrderReason)))
            .NotEmpty().WithMessage("دلیل بازگشت کالا اجباریست");
        RuleFor(v => v.ItemGroups.Select(c => c.OrderItems.Select(e => e.ReturnOrderReason.Details)))
            .NotEmpty().WithMessage("توضیحات دلیل بازگشت اجباریست");

        RuleFor(v =>
                v.ItemGroups.Select(
                    c => c.OrderItems.Select(e => e.ReturnOrderItemDocuments.Select(t => t.Description))))
            .NotEmpty().WithMessage("توضیحات  مستند اجباریست");
        RuleFor(v =>
                v.ItemGroups.Select(c => c.OrderItems.Select(e => e.ReturnOrderItemDocuments.Select(t => t.Image))))
            .NotEmpty().WithMessage("تصویر مستند اجباریست");
        
    }
}

