using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.CreateProductOptionColor;
public record CreateProductOptionColorCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
}



public class CreateProductOptionColorCommandHandler : IRequestHandler<CreateProductOptionColorCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionColorCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionColor()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
        };
        _context.ProductOptionColors.Add(entity);


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
