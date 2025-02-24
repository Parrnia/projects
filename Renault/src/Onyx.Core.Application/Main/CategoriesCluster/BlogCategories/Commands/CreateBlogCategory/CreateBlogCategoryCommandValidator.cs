using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.CreateBlogCategory;
public class CreateBlogCategoryCommandValidator : AbstractValidator<CreateBlogCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateBlogCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("دسته بندی بلاگی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(100).WithMessage("نام فارسی نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("دسته بندی بلاگی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(100).WithMessage("نام لاتین نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.Image)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه تصویر دسته بندی بلاگ اجباریست");

    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.BlogCategories
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.BlogCategories
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
