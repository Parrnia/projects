using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.UpdateProductBrand;
public class UpdateProductBrandCommandValidator : AbstractValidator<UpdateProductBrandCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateProductBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.BrandLogo)
            .NotEmpty().WithMessage("تصویر برند اجباریست");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("برندی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(50).WithMessage("نام فارسی نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("برندی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.ProductBrands
            .Where(c => c.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductBrands
            .Where(c => c.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}