using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.DeleteProductTypeAttributeGroup;

public class DeleteRangeProductTypeAttributeGroupsCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductTypeAttributeGroupsCommandHandler : IRequestHandler<DeleteRangeProductTypeAttributeGroupsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductTypeAttributeGroupsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductTypeAttributeGroupsCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
                .Include(c => c.ProductAttributeTypes)
                .ThenInclude(c => c.Products)
                .ThenInclude(c => c.Attributes)
                .Include(c => c.Attributes)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (productTypeAttributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), id);
            }

            productTypeAttributeGroup.HandleRemoveProductTypeAttributeGroup();

            _context.ProductTypeAttributeGroups.Remove(productTypeAttributeGroup);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}