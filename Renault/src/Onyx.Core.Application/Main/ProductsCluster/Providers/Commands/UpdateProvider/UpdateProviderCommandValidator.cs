using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Commands.UpdateProvider;
public class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateProviderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("تامین کننده ای با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(100).WithMessage("نام فارسی نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("چیزی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(100).WithMessage("نام لاتین نباید بیشتر از 100 کاراکتر باشد");
        RuleFor(v => v.LocalizedCode)
            .MaximumLength(20).WithMessage("نام فارسی نباید بیشتر از 20 کاراکتر باشد");
        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("توصیف اطلاعات نباید بیشتر از 500 کاراکتر باشد");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Providers
            .Where(l => l.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Providers
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}