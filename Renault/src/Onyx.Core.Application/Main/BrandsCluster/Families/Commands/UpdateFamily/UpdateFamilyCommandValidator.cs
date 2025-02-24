using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Commands.UpdateFamily;
public class UpdateFamilyCommandValidator : AbstractValidator<UpdateFamilyCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateFamilyCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("خانواده ای با این نام فارسی موجود است")
            .MaximumLength(250).WithMessage("نام فارسی نباید بیشتر از 250 کاراکتر باشد")
            .NotEmpty().WithMessage("نام فارسی اجباریست");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("خانواده ای با این نام لاتین موجود است")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.VehicleBrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Families
            .Where(c => c.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Families
            .Where(c => c.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
