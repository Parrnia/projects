using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Validators;

public record UniqueCarouselTitleValidator : IRequest<bool>
{
    public int CarouselId { get; init; }
    public string CarouselTitle { get; init; } = null!;
}

public class UniqueCarouselTitleValidatorHandler : IRequestHandler<UniqueCarouselTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCarouselTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCarouselTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Carousels
            .Where(c => c.Id != request.CarouselId)
            .AllAsync(e => e.Title != request.CarouselTitle, cancellationToken);
        return isUnique;
    }
}
