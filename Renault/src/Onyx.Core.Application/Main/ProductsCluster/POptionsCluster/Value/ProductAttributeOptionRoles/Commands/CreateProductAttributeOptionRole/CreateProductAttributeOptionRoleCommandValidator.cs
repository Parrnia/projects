using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.CreateProductAttributeOptionRole;
public class CreateProductAttributeOptionRoleCommandValidator : AbstractValidator<CreateProductAttributeOptionRoleCommand>
{
    private readonly IApplicationDbContext _context;
    private int _productAttributeOptionId;

    public CreateProductAttributeOptionRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

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
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست")
            .MustAsync(GetProductAttributeOptionIdForUniqueness);
        RuleFor(v => v.CustomerTypeEnumId)
            .IsInEnum().WithMessage("نوع مشتری اجباریست")
            .MustAsync(BeUniqueProductAttributeOptionRoleByCustomerTypeEnum).WithMessage("نقش آپشن محصولی با این نوع مشتری موجود است");
        RuleFor(v => v.DiscountPercent)
            .GreaterThanOrEqualTo(0).WithMessage("درصد تخفیف اجباریست");
    }

    public async Task<bool> BeUniqueProductAttributeOptionRoleByCustomerTypeEnum(int customerTypeEnum, CancellationToken cancellationToken)
    {
        var result = await _context.ProductAttributeOptionRoles
            .Where(l => l.ProductAttributeOptionId == _productAttributeOptionId)
            .AllAsync(l => l.CustomerTypeEnum != (CustomerTypeEnum)customerTypeEnum, cancellationToken);
        return result;
    }
    public async Task<bool> GetProductAttributeOptionIdForUniqueness(int productAttributeOptionId, CancellationToken cancellationToken)
    {
        _productAttributeOptionId = productAttributeOptionId;
        return await Task.FromResult(true);
    }
}
