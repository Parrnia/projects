using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Validators;

public record UniqueTagEnTitleValidator : IRequest<bool>
{
    public int TagId { get; init; }
    public string EnTitle { get; init; } = null!;
}

public class UniqueTagEnTitleValidatorHandler : IRequestHandler<UniqueTagEnTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueTagEnTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueTagEnTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Tags.Where(c => c.Id != request.TagId)
            .AllAsync(e => e.EnTitle != request.EnTitle, cancellationToken);
        return isUnique;
    }
}
