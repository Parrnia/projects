using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.DeleteCountingUnit;

public record DeleteCountingUnitCommand(int Id) : IRequest<Unit>;

public class DeleteCountingUnitCommandHandler : IRequestHandler<DeleteCountingUnitCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCountingUnitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCountingUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CountingUnits
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CountingUnit), request.Id);
        }

        _context.CountingUnits.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}