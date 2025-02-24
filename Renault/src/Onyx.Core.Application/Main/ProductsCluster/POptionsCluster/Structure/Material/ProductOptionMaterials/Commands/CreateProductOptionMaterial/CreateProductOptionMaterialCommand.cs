using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.CreateProductOptionMaterial;
public record CreateProductOptionMaterialCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
}



public class CreateProductOptionMaterialCommandHandler : IRequestHandler<CreateProductOptionMaterialCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionMaterial()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
        };
        _context.ProductOptionMaterials.Add(entity);


        var attributeGroup = new ProductTypeAttributeGroup
        {
            Name = entity.Name,
            Slug = entity.Slug,
            Attributes = new List<ProductTypeAttributeGroupAttribute>()
            {
                new ProductTypeAttributeGroupAttribute(EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(entity.Type))
            }
        };
        _context.ProductTypeAttributeGroups.Add(attributeGroup);


        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
