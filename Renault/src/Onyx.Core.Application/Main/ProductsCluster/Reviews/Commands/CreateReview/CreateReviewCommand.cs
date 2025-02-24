using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Commands.CreateReview;
public record CreateReviewCommand : IRequest<int>
{
    public int Rating { get; init; }
    public string Content { get; init; } = null!;
    public string AuthorName { get; set; } = null!;
    public int ProductId { get; init; }
    public Guid CustomerId { get; set; }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var entity = new Review()
        {
            Rating = request.Rating,
            Content = request.Content,
            AuthorName = request.AuthorName,
            ProductId = request.ProductId,
            CustomerId = request.CustomerId,
            IsActive = false
        };

        _context.Reviews.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
