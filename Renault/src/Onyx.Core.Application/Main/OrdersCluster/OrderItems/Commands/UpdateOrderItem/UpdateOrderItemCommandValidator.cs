using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Commands.UpdateOrderItem;
public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
{
    private readonly IApplicationDbContext _context;
    private int _productAttributeOptionId;
    private CustomerTypeEnum _customerTypeEnum;
    public UpdateOrderItemCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ProductAttributeOptionId)
            .MustAsync(GetProductAttributeOptionId)
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");
        RuleFor(v => v.CustomerTypeEnum)
            .MustAsync(GetCustomerType)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
        RuleFor(v => v.Price)
            .NotEmpty().WithMessage("قیمت اجباریست");
        RuleFor(v => v.DiscountPercentForProduct)
            .NotEmpty().WithMessage("درصد تخفیف بر روی محصول اجباریست");
        RuleFor(v => v.Quantity)
            .MustAsync(CheckAvailability).WithMessage("تعداد درخواستی از تعداد موجود در انبار بیشتر است")
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
    public Task<bool> GetProductAttributeOptionId(int productAttributeOptionId, CancellationToken cancellationToken)
    {
        this._productAttributeOptionId = productAttributeOptionId;
        return Task.FromResult(true);
    }
    public async Task<bool> GetCustomerType(CustomerTypeEnum customerTypeEnum, CancellationToken cancellationToken)
    {
        _customerTypeEnum = customerTypeEnum;
        return await Task.FromResult(true);
    }
    public async Task<bool> CheckAvailability(double quantity, CancellationToken cancellationToken)
    {
        var entity = await this._context
            .ProductAttributeOptions
            .Include(c => c.ProductAttributeOptionRoles)
            .SingleOrDefaultAsync(e => e.Id == _productAttributeOptionId, cancellationToken);
        if (entity?.TotalCount < quantity ||
            entity?.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == _customerTypeEnum)?.CurrentMaxOrderQty < quantity ||
            entity?.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == _customerTypeEnum)?.CurrentMinOrderQty > quantity)
        {
            await Task.FromResult(false);
        }
        return await Task.FromResult(true);
    }
}
