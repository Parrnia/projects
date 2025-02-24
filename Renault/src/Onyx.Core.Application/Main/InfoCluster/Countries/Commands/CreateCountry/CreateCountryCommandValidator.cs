using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Commands.CreateCountry;
public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCountryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.LocalizedName)
            .MustAsync(BeUniqueLocalizedName).WithMessage("کشوری با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(50).WithMessage("نام فارسی نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("کشوری با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست")
            .MaximumLength(50).WithMessage("نام لاتین نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("فیلد شمارنده اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
