using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Validators;

public record UniqueKindLocalizedNameValidator : IRequest<bool>
{
    public int KindId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueKindLocalizedNameValidatorHandler : IRequestHandler<UniqueKindLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueKindLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueKindLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        //var isUnique = await _context.Kinds.Where(c => c.Id != request.KindId)
        //    .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        //return isUnique;
        return true;
    }
}
    