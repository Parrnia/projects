using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.CreateProductOptionValueColor;
public record CreateProductOptionValueColorCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
    public int ProductOptionColorId { get; init; }

}

public class CreateProductOptionValueColorCommandHandler : IRequestHandler<CreateProductOptionValueColorCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionValueColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionValueColorCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionValueColor()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
            Color = request.Color,
            ProductOptionColorId = request.ProductOptionColorId,
        };

        _context.ProductOptionValueColors.Add(entity);


        var products = _context.Products
            .Where(c => c.ColorOptionId == request.ProductOptionColorId)
            .Include(c => c.Attributes);

        foreach (var product in products)
        {
            product.Attributes.Add(new ProductAttribute()
                {
                    Name = request.Name,
                    Slug = request.Name.ToLower().Replace(' ', '-'),
                    ValueName = "",
                    ValueSlug = "",
                    Featured = false
                });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
