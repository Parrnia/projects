using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.CreateBadge;
public class CreateBadgeCommandValidator : AbstractValidator<CreateBadgeCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateBadgeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست")
            .MustAsync(BeUniqueValue).WithMessage("نشانی با این مقدار موجود است");
    }

    public async Task<bool> BeUniqueValue(string value, CancellationToken cancellationToken)
    {
        return await _context.Badges
            .AllAsync(l => l.Value != value, cancellationToken);
    }
}
