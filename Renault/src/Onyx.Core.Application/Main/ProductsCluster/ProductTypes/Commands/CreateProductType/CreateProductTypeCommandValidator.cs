using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.CreateProductType;
public class CreateProductTypeCommandValidator : AbstractValidator<CreateProductTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("نوع محصولی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("نوع محصولی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(50).WithMessage("نام فارسی نباید بیشتر از 50 کاراکتر باشد");
        
        
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
