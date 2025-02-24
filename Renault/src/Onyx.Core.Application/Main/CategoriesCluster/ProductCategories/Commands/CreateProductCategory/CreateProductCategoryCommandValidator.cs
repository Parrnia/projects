using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.CreateProductCategory;
public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;


    public CreateProductCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

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
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
