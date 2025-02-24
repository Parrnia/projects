using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.CreateProductAttributeOptionValue;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.CreateProductAttributeOption;

public class CreateProductAttributeOptionCommandValidator : AbstractValidator<CreateProductAttributeOptionCommand>
{
    private readonly IApplicationDbContext _context;
    private Product _product = null!;

    public CreateProductAttributeOptionCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست")
            .MustAsync(ValidateProductAttributeOptionsCount).WithMessage("همه ی ترکیب های ممکن انتخاب شده است، لطفا ترکیب های موجود را تغییر دهید");
        RuleFor(v => v.TotalCount)
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.SafetyStockQty)
            .NotEmpty().WithMessage("مقدار ذخیره احتیاطی اجباریست");
        RuleFor(v => v.MinStockQty)
            .NotEmpty().WithMessage("مقدار حداقل موجودی اجباریست");
        RuleFor(v => v.MaxStockQty)
            .NotEmpty().WithMessage("مقدار حداکثر موجودی اجباریست");

        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(ValidateProductAttributeOptionValuesCount).WithMessage("تعداد مقدارهای آپشن ها نامعتبر هستند");

        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(list => list.Count == 0 || list.All(e => !string.IsNullOrWhiteSpace(e.Name)))
            .WithMessage("نام اجباریست");
        RuleFor(v => v.ProductAttributeOptionValues)
            .Must(list => list.Count == 0 || list.All(e => !string.IsNullOrWhiteSpace(e.Value)))
            .WithMessage("مقدار اجباریست");
    }


    public async Task<bool> ValidateProductAttributeOptionsCount(int productId, CancellationToken cancellationToken)
    {
        _product = await _context.Products
                       .Include(c => c.AttributeOptions)
                       .Include(c => c.ColorOption)
                       .ThenInclude(c => c.Values)
                       .Include(c => c.MaterialOption)
                       .ThenInclude(c => c.Values)
                       .SingleOrDefaultAsync(c => c.Id == productId, cancellationToken: cancellationToken)
                   ?? throw new NotFoundException(nameof(Product), productId);

        var validCount = 1;

        if (_product.ColorOption != null)
        {
            validCount *= _product.ColorOption.Values.Count;
        }

        if (_product.MaterialOption != null)
        {
            validCount *= _product.MaterialOption.Values.Count;
        }

        return _product.AttributeOptions.Count < validCount;
    }
    public bool ValidateProductAttributeOptionValuesCount(IList<CreateProductAttributeOptionValueCommand> cmds)
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