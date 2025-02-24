using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Products.Commands.UpdateProduct;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    private int? _colorOptionId;
    private int? _materialOptionId;
    private int? _defaultOptionId;
    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.ProductNo)
            .MaximumLength(70).WithMessage("کد کالا نباید بیشتر از 70 کاراکتر باشد");
        RuleFor(v => v.OldProductNo)
            .MaximumLength(70).WithMessage("کد کالای قدیمی نباید بیشتر از 70 کاراکتر باشد");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("چیزی با این نام فارسی موجود است")
            .MaximumLength(100).WithMessage("نام فارسی نباید بیشتر از 100 کاراکتر باشد")
            .NotEmpty().WithMessage("نام فارسی اجباریست");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("چیزی با این نام لاتین موجود است")
            .MaximumLength(100).WithMessage("نام لاتین نباید بیشتر از 100 کاراکتر باشد")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.OrderRate)
            .NotEmpty().WithMessage("ضریب سفارش دهی اجباریست");
        RuleFor(v => v.ProductBrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");
        RuleFor(v => v.ProductCategoryId)
            .NotEmpty().WithMessage("شناسه دسته بندی محصول اجباریست");
        RuleFor(v => v.ProductAttributeTypeId)
            .NotEmpty().WithMessage("شناسه ویژگی محصول اجباریست");
        RuleFor(v => v.Excerpt)
            .NotEmpty().WithMessage("گزیده اطلاعات اجباریست");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("توصیف اطلاعات اجباریست");
        //RuleFor(v => v.Compatibility)
        //    .NotEmpty().WithMessage("سازگاری اجباریست");


        RuleFor(v => v.ProductOptionColorId)
            .MustAsync(GetColorOptionId);
        RuleFor(v => v.ProductOptionMaterialId)
            .MustAsync(GetMaterialOptionId);
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(l => l.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
    public Task<bool> GetColorOptionId(int? requestId, CancellationToken cancellationToken)
    {
        this._colorOptionId = requestId;
        return Task.FromResult(true);
    }
    public Task<bool> GetMaterialOptionId(int? requestId, CancellationToken cancellationToken)
    {
        this._materialOptionId = requestId;
        return Task.FromResult(true);
    }
    //public Task<bool> GetDefaultOptionId(int? requestId, CancellationToken cancellationToken)
    //{
    //    this._defaultOptionId = requestId;
    //    return Task.FromResult(((_colorOptionId != null || _materialOptionId != null) && _defaultOptionId == null) ||
    //                           (_defaultOptionId != null && (_colorOptionId == null && _materialOptionId == null)));
    //}
}