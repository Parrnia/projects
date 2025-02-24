using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.UpdateProductOptionMaterial;
public record UpdateProductOptionMaterialCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateProductOptionMaterialCommandHandler : IRequestHandler<UpdateProductOptionMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionMaterials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionMaterial), request.Id);
        }

        var attributeGroup = await _context.ProductTypeAttributeGroups
            .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

        if (attributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
        }

        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');

        attributeGroup.Name = entity.Name;
        attributeGroup.Slug = entity.Slug;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}