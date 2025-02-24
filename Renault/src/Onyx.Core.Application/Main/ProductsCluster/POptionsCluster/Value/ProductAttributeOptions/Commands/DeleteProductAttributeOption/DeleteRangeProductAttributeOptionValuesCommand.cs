using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.DeleteProductAttributeOption;

public class DeleteRangeProductAttributeOptionValuesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}


public class DeleteRangeProductAttributeOptionValuesCommandHandler : IRequestHandler<DeleteRangeProductAttributeOptionValuesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductAttributeOptionValuesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductAttributeOptionValuesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductAttributeOptionValues
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductAttributeOptionValue), id);
            }

            _context.ProductAttributeOptionValues.Remove(entity);

            //entity.AddDomainEvent(new KindsDe(entity));
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}