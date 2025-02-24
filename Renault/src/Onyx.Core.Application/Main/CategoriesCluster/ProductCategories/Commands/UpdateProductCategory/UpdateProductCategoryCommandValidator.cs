using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.UpdateProductCategory;
public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateProductCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("دسته بندی کالایی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(100).WithMessage("نام فارسی نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("دسته بندی کالایی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(100).WithMessage("نام لاتین نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.ProductCategoryNo)
            .MaximumLength(70).WithMessage("شماره دسته کالا نباید بیشتر از 70 کاراکتر باشد");
        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("فیلد تصویر اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .Where(l => l.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}