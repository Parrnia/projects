using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.DeleteProductOptionValueMaterial;

public record DeleteProductOptionValueMaterialCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionValueMaterialCommandHandler : IRequestHandler<DeleteProductOptionValueMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionValueMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionValueMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionValueMaterials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionValueMaterial), request.Id);
        }

        _context.ProductOptionValueMaterials.Remove(entity);

        var products = await _context.Products
            .Where(c => c.MaterialOptionId == request.Id)
            .Include(c => c.Attributes).ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == entity.Name);
            if (dbAttribute != null)
            {
                _context.ProductAttributes.Remove(dbAttribute);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}