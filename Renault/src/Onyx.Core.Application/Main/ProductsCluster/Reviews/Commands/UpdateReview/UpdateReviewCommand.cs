using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Commands.UpdateReview;
public record UpdateReviewCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Rating { get; init; }
    public string Content { get; init; } = null!;
    public string AuthorName { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Reviews
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }
        
        entity.Rating = request.Rating;
        entity.Content = request.Content;
        entity.AuthorName = request.AuthorName;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}