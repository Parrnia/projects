using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.UpdateBadge;
public class UpdateBadgeCommandValidator : AbstractValidator<UpdateBadgeCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateBadgeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست")
            .MustAsync(BeUniqueValue).WithMessage("نشانی با این مقدار موجود است");
    }

    public async Task<bool> BeUniqueValue(string value, CancellationToken cancellationToken)
    {
        return await _context.Badges
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Value != value, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}