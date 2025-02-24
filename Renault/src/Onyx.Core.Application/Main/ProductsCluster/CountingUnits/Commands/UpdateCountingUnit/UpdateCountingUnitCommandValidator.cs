using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.UpdateCountingUnit;
public class UpdateCountingUnitCommandValidator : AbstractValidator<UpdateCountingUnitCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateCountingUnitCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.LocalizedName)
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MustAsync(BeUniqueLocalizedName).WithMessage("واحد شمارشی با این نام فارسی موجود است")
            .MaximumLength(50).WithMessage("نام فارسی نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("چیزی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits
            .Where(l => l.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}