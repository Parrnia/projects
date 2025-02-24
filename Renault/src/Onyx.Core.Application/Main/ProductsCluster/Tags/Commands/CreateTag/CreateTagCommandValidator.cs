using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Commands.CreateTag;
public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.EnTitle)
            .MustAsync(BeUniqueEnTitle).WithMessage("تگی با این عنوان لاتین موجود است")
            .NotEmpty().WithMessage("عنوان لاتین اجباریست");
        RuleFor(v => v.FaTitle)
            .MustAsync(BeUniqueFaTitle).WithMessage("تگی با این عنوان فارسی موجود است")
            .NotEmpty().WithMessage("عنوان فارسی اجباریست");
    }

    public async Task<bool> BeUniqueEnTitle(string enTitle, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AllAsync(l => l.EnTitle != enTitle, cancellationToken);
    }
    public async Task<bool> BeUniqueFaTitle(string faTitle, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AllAsync(l => l.FaTitle != faTitle, cancellationToken);
    }
}
