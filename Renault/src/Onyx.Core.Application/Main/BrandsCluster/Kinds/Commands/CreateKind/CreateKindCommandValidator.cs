using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Commands.CreateKind;
public class CreateKindCommandValidator : AbstractValidator<CreateKindCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateKindCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.LocalizedName)
            //.MustAsync(BeUniqueLocalizedName).WithMessage("نوعی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(250).WithMessage("نام فارسی نباید بیشتر از 250 کاراکتر باشد");
        RuleFor(v => v.Name)
            //.MustAsync(BeUniqueName).WithMessage("نوعی با این نام لاتین موجود است")
            .MaximumLength(250).WithMessage("نام لاتین نباید بیشتر از 250 کاراکتر باشد")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.ModelId)
            .NotEmpty().WithMessage("شناسه مدل اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Kinds
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Kinds
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}