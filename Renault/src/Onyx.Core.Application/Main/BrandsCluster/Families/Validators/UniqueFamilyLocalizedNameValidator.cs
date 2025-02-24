using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Validators;

public record UniqueFamilyLocalizedNameValidator : IRequest<bool>
{
    public int FamilyId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueFamilyLocalizedNameValidatorHandler : IRequestHandler<UniqueFamilyLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueFamilyLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueFamilyLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Families.Where(c => c.Id != request.FamilyId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
    