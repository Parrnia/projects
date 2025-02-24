using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Validators;

public record UniqueModelNameValidator : IRequest<bool>
{
    public int ModelId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueModelNameValidatorHandler : IRequestHandler<UniqueModelNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueModelNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueModelNameValidator request, CancellationToken cancellationToken)
    {
        //var isUnique = await _context.Models.Where(c => c.Id != request.ModelId)
        //    .AllAsync(e => e.Name != request.Name, cancellationToken);
        //return isUnique;
        return true;
    }
}
    