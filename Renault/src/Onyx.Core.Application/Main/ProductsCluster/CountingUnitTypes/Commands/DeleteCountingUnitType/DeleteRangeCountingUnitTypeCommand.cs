using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.DeleteCountingUnitType;

public class DeleteRangeCountingUnitTypeCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeCountingUnitTypeCommandHandler : IRequestHandler<DeleteRangeCountingUnitTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeCountingUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeCountingUnitTypeCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.CountingUnitTypes
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(CountingUnitType), id);
            }

            _context.CountingUnitTypes.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
