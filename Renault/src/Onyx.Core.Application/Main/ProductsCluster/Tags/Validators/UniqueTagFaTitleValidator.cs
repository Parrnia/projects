using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Validators;

public record UniqueTagFaTitleValidator : IRequest<bool>
{
    public int TagId { get; init; }
    public string FaTitle { get; init; } = null!;
}

public class UniqueTagFaTitleValidatorHandler : IRequestHandler<UniqueTagFaTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueTagFaTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueTagFaTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Tags.Where(c => c.Id != request.TagId)
            .AllAsync(e => e.FaTitle != request.FaTitle, cancellationToken);
        return isUnique;
    }
}
    