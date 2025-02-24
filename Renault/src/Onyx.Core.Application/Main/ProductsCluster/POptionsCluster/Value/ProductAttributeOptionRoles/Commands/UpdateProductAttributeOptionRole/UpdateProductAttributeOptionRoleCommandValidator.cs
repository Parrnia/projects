using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.UpdateProductAttributeOptionRole;
public class UpdateProductAttributeOptionRoleCommandValidator : AbstractValidator<UpdateProductAttributeOptionRoleCommand>
{
    private readonly IApplicationDbContext _context;
    private int _productAttributeOptionId;
    private int _id;

    public UpdateProductAttributeOptionRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .MustAsync(GetId);
        RuleFor(v => v.MinimumStockToDisplayProductForThisCustomerTypeEnum)
            .GreaterThanOrEqualTo(0)
            .WithMessage("حداقل موجودی کالا برای نمایش کالا به کاربر اجباریست");
        RuleFor(v => v.MainMaxOrderQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("حداکثر مقدار سفارش گذاری اصلی اجباریست");
        RuleFor(v => v.MainMinOrderQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("حداقل مقدار سفارش گذاری اصلی اجباریست");
        RuleFor(v => v.ProductAttributeOptionId)
            .GreaterThanOrEqualTo(0).WithMessage("شناسه نوع آپشن محصول اجباریست")
            .MustAsync(GetProductAttributeOptionIdForUniqueness);
        RuleFor(v => v.CustomerTypeEnumId)
            .GreaterThanOrEqualTo(0).WithMessage("نوع مشتری اجباریست")
            .MustAsync(BeUniqueProductAttributeOptionRoleByCustomerTypeEnum).WithMessage("نقش آپشن محصولی با این نوع مشتری موجود است");
        RuleFor(v => v.DiscountPercent)
            .GreaterThanOrEqualTo(0)
            .WithMessage("درصد تخفیف اجباریست");
    }

    public async Task<bool> BeUniqueProductAttributeOptionRoleByCustomerTypeEnum(int customerTypeEnum, CancellationToken cancellationToken)
    {
        var result = await _context.ProductAttributeOptionRoles
            .Where(l => l.Id != _id && l.ProductAttributeOptionId == _productAttributeOptionId)
            .AllAsync(l => l.CustomerTypeEnum != (CustomerTypeEnum) customerTypeEnum, cancellationToken);
        return result;
    }
    public async Task<bool> GetProductAttributeOptionIdForUniqueness(int productAttributeOptionId, CancellationToken cancellationToken)
    {
        _productAttributeOptionId = productAttributeOptionId;
        return await Task.FromResult(true);
    }
    public async Task<bool> GetId(int id, CancellationToken cancellationToken)
    {
        _id = id;
        return await Task.FromResult(true);
    }
}