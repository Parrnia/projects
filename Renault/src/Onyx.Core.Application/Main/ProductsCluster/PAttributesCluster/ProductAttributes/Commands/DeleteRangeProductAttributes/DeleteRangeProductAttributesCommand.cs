using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.DeleteRangeProductAttributes;

public class DeleteRangeProductAttributesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductAttributesCommandHandler : IRequestHandler<DeleteRangeProductAttributesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductAttributesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductAttributesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductAttributes
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductAttribute), id);
            }

            _context.ProductAttributes.Remove(entity);

            //entity.AddDomainEvent(new KindsDe(entity));
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
