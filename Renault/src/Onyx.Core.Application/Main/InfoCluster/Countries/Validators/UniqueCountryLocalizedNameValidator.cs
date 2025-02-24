using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Validators;

public record UniqueCountryLocalizedNameValidator : IRequest<bool>
{
    public int CountryId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueCountryLocalizedNameValidatorHandler : IRequestHandler<UniqueCountryLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountryLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountryLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Countries.Where(c => c.Id != request.CountryId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
