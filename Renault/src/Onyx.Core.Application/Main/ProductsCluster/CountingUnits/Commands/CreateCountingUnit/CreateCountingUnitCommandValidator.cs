using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.CreateCountingUnit;
public class CreateCountingUnitCommandValidator : AbstractValidator<CreateCountingUnitCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCountingUnitCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
        RuleFor(v => v.LocalizedName)
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(50).WithMessage("نام فارسی نباید بیشتر از 50 کاراکتر باشد")
            .MustAsync(BeUniqueLocalizedName).WithMessage("واحد شمارشی با این نام فارسی موجود است");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MustAsync(BeUniqueName).WithMessage("واحد شمارشی با این نام لاتین موجود است")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
