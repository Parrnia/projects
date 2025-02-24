using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.DeleteProductAttributeType;

public class DeleteRangeProductAttributeTypesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductAttributeTypesCommandHandler : IRequestHandler<DeleteRangeProductAttributeTypesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductAttributeTypesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductAttributeTypesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductAttributeTypes
                .Include(c => c.AttributeGroups)
                .ThenInclude(c => c.Attributes)
                .Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductAttributeType), id);
            }

            entity.HandleRemoveProductAttributeType();

            _context.ProductAttributeTypes.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}