using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Posts.Validators;

public record UniquePostTitleValidator : IRequest<bool>
{
    public int PostId { get; init; }
    public string Title { get; init; } = null!;
}

public class UniquePostTitleValidatorHandler : IRequestHandler<UniquePostTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniquePostTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniquePostTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Posts.Where(c => c.Id != request.PostId)
            .AllAsync(e => e.Title != request.Title, cancellationToken);
        return isUnique;
    }
}
