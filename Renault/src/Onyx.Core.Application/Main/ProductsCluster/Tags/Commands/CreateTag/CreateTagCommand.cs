using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Tags.Commands.CreateTag;
public record CreateTagCommand : IRequest<int>
{
    public string EnTitle { get; init; } = null!;
    public string FaTitle { get; init; } = null!;
    public bool IsActive { get; init; }
    public IList<int> ProductIds { get; set; } = new List<int>();
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var products = new List<Product>();
        foreach (var productId in request.ProductIds)
        {
            var product = await _context.Products.FindAsync(productId, cancellationToken) ?? throw new NotFoundException(nameof(Product), productId);
            products.Add(product);
        }
        var entity = new Tag()
        {
            EnTitle = request.EnTitle,
            FaTitle = request.FaTitle,
            IsActive = request.IsActive,
            Products = products
        };


        _context.Tags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
