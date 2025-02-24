using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.UpdateProductOptionValueMaterial;
public record UpdateProductOptionValueMaterialCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public int ProductOptionMaterialId { get; init; }
}

public class UpdateProductOptionValueMaterialCommandHandler : IRequestHandler<UpdateProductOptionValueMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionValueMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionValueMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionValueMaterials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionValueMaterial), request.Id);
        }

        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');
        entity.ProductOptionMaterialId = request.ProductOptionMaterialId;

        var products = await _context.Products
            .Where(c => c.MaterialOptionId == request.ProductOptionMaterialId)
            .Include(c => c.Attributes).ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == entity.Name);
            if (dbAttribute != null)
            {
                dbAttribute.Name = request.Name;
                dbAttribute.Slug = request.Name.ToLower().Replace(' ', '-');
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}