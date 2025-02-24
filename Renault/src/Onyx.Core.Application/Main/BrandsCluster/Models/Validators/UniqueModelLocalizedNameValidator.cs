using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Validators;

public record UniqueModelLocalizedNameValidator : IRequest<bool>
{
    public int ModelId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueModelLocalizedNameValidatorHandler : IRequestHandler<UniqueModelLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueModelLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueModelLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        //var isUnique = await _context.Models.Where(c => c.Id != request.ModelId)
        //    .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        //return isUnique;
        return true;
    }
}
    