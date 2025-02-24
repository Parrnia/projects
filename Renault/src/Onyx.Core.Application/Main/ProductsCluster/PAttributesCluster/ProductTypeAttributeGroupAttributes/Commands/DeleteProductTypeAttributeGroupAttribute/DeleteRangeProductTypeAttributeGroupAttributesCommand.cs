using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.DeleteProductTypeAttributeGroupAttribute;

public class DeleteRangeProductTypeAttributeGroupAttributesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductTypeAttributeGroupAttributesCommandHandler : IRequestHandler<DeleteRangeProductTypeAttributeGroupAttributesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductTypeAttributeGroupAttributesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductTypeAttributeGroupAttributesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductTypeAttributeGroupAttributes
                .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroupAttribute), id);
            }

            var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
                .Include(c => c.ProductAttributeTypes)
                .ThenInclude(c => c.Products)
                .ThenInclude(c => c.Attributes)
                .FirstOrDefaultAsync(x => x.Id == entity.ProductTypeAttributeGroupId, cancellationToken);

            if (productTypeAttributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.ProductTypeAttributeGroupId);
            }

            var attributes = new List<ProductTypeAttributeGroupAttribute> { entity };
            productTypeAttributeGroup.RemoveAttributes(attributes);

            _context.ProductTypeAttributeGroupAttributes.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}