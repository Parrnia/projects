using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Validators;

public record UniqueCountryNameValidator : IRequest<bool>
{
    public int CountryId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueCountryNameValidatorHandler : IRequestHandler<UniqueCountryNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountryNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountryNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Countries.Where(c => c.Id != request.CountryId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    