using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.DeleteProductType;

public class DeleteRangeProductTypesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}



public class DeleteRangeProductTypesCommandHandler : IRequestHandler<DeleteRangeProductTypesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductTypesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductTypesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductTypes
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductType), id);
            }

            _context.ProductTypes.Remove(entity);

            //entity.AddDomainEvent(new KindsDe(entity));
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}