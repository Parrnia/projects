using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.CreateProductOptionValueMaterial;
public record CreateProductOptionValueMaterialCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public int ProductOptionMaterialId { get; init; }

}

public class CreateProductOptionValueMaterialCommandHandler : IRequestHandler<CreateProductOptionValueMaterialCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionValueMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionValueMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionValueMaterial()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
            ProductOptionMaterialId = request.ProductOptionMaterialId,
        };

        _context.ProductOptionValueMaterials.Add(entity);

        var products = _context.Products
            .Where(c => c.MaterialOptionId == request.ProductOptionMaterialId)
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
