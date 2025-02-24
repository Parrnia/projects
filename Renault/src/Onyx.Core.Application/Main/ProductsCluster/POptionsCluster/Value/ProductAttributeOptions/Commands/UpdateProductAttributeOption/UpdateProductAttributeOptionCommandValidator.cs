using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.UpdateProductAttributeOption;

public class UpdateProductAttributeOptionCommandValidator : AbstractValidator<UpdateProductAttributeOptionCommand>
{
    private readonly IApplicationDbContext _context;
    private Product _product = null!;

    public UpdateProductAttributeOptionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetProductId).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.TotalCount)
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.SafetyStockQty)
            .NotEmpty().WithMessage("مقدار ذخیره احتیاطی اجباریست");
        RuleFor(v => v.MinStockQty)
            .NotEmpty().WithMessage("مقدار حداقل موجودی اجباریست");
        RuleFor(v => v.MaxStockQty)
            .NotEmpty().WithMessage("مقدار حداکثر موجودی اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");

        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(ValidateProductAttributeOptionValuesCount).WithMessage("تعداد مقدارهای آپشن ها نامعتبر هستند");

        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(list => list.Count == 0 || list.All(e => !string.IsNullOrWhiteSpace(e.Name)))
            .WithMessage("نام اجباریست");
        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(list => list.Count == 0 || list.All(e => !string.IsNullOrWhiteSpace(e.Value)))
            .WithMessage("مقدار اجباریست");

    }

    public async Task<bool> GetProductId(int productId, CancellationToken cancellationToken)
    {
        _product = await _context.Products
                       .Include(c => c.ColorOption)
                       .Include(c => c.MaterialOption)
                       .SingleOrDefaultAsync(c => c.Id == productId, cancellationToken: cancellationToken)
                   ?? throw new NotFoundException(nameof(Product), productId);
        return true;
    }

    public bool ValidateProductAttributeOptionValuesCount(IList<UpdateProductAttributeOptionValueCommand> cmds)
    {
        var validCount = 0;

        if (_product.ColorOption != null)
        {
            validCount++;
        }
        if (_product.MaterialOption != null)
        {
            validCount++;
        }

        return cmds.Count == validCount;
    }
}