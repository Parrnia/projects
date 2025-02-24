using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Validators;

public record UniqueAboutUsTitleValidator : IRequest<bool>
{
    public int AboutUsId { get; init; }
    public string Title { get; init; } = null!;
}

public class UniqueAboutUsTitleValidatorHandler : IRequestHandler<UniqueAboutUsTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueAboutUsTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueAboutUsTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.AboutUsEnumerable.Where(c => c.Id != request.AboutUsId)
            .AllAsync(e => e.Title != request.Title, cancellationToken);
        return isUnique;
    }
}
