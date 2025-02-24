using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.DeleteProductAttributeOption;

public class DeleteRangeProductAttributeOptionCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}
public class DeleteRangeProductAttributeOptionCommandHandler : IRequestHandler<DeleteRangeProductAttributeOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductAttributeOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductAttributeOptionCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductAttributeOptions
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductAttributeOption), id);
            }

            _context.ProductAttributeOptions.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}