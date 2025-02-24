using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Validators;

public record UniqueKindNameValidator : IRequest<bool>
{
    public int KindId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueKindNameValidatorHandler : IRequestHandler<UniqueKindNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueKindNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueKindNameValidator request, CancellationToken cancellationToken)
    {
        //var isUnique = await _context.Kinds.Where(c => c.Id != request.KindId)
        //    .AllAsync(e => e.Name != request.Name, cancellationToken);
        //return isUnique;
        return true;
    }
}
    