using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Tags.Commands.UpdateTag;
public record UpdateTagCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string EnTitle { get; init; } = null!;
    public string FaTitle { get; init; } = null!;
    public bool IsActive { get; set; }
    public IList<int> ProductIds { get; set; } = new List<int>();
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Tag), request.Id);
        }
        var products = new List<Product>();
        foreach (var productId in request.ProductIds)
        {
            var product = await _context.Products.FindAsync(productId, cancellationToken) ?? throw new NotFoundException(nameof(Product), productId);
            products.Add(product);
        }

        entity.EnTitle = request.EnTitle;
        entity.FaTitle = request.FaTitle;
        entity.Products = products;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}