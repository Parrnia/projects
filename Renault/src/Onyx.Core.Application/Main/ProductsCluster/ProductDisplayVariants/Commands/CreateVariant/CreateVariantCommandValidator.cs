using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.CreateVariant;
public class CreateVariantCommandValidator : AbstractValidator<CreateVariantCommand>
{
    private readonly IApplicationDbContext _context;
    private int _productId;
    public CreateVariantCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ProductId)
            .MustAsync(GetProductIdForUniqueness)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام نمایشی اجباریست")
            .MustAsync(BeUniqueName).WithMessage("نامی با این مقدار برای این محصول موجود است");
        
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductDisplayVariants
            .Where(l => l.ProductId == _productId)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetProductIdForUniqueness(int productId, CancellationToken cancellationToken)
    {
        this._productId = productId;
        return Task.FromResult(true);
    }
}
