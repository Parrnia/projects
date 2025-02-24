using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.AddProductAttributeGroups;
public record AddProductAttributeGroupsCommand : IRequest<Unit>
{ 
    public int Id { get; init; }
    public IList<int> AttributeGroupIds { get; set; } = new List<int>();
}

public class AddProductAttributeGroupsCommandHandler : IRequestHandler<AddProductAttributeGroupsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddProductAttributeGroupsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddProductAttributeGroupsCommand request, CancellationToken cancellationToken)
    {
        var attributeGroups = new List<ProductTypeAttributeGroup>();
        foreach (var attributeGroupId in request.AttributeGroupIds)
        {
            var attributeGroup = await _context.ProductTypeAttributeGroups
                .Include(c => c.Attributes)
                .SingleOrDefaultAsync(c=> c.Id == attributeGroupId, cancellationToken) ?? throw new NotFoundException(nameof(ProductTypeAttributeGroup), attributeGroupId);
            attributeGroups.Add(attributeGroup);
        }


        var entity = await _context.ProductAttributeTypes
            .Include(c => c.AttributeGroups)
            .ThenInclude(c => c.Attributes)
            .Include(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .SingleOrDefaultAsync( c => c.Id == request.Id , cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeType), request.Id);
        }


        entity.AddAttributeGroups(attributeGroups);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
