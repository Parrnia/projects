using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Validators;

public record UniqueFamilyNameValidator : IRequest<bool>
{
    public int FamilyId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueFamilyNameValidatorHandler : IRequestHandler<UniqueFamilyNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueFamilyNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueFamilyNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Families.Where(c => c.Id != request.FamilyId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    