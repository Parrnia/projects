using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.DeleteCountingUnitType;

public record DeleteCountingUnitTypeCommand(int Id) : IRequest<Unit>;

public class DeleteCountingUnitTypeCommandHandler : IRequestHandler<DeleteCountingUnitTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCountingUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCountingUnitTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CountingUnitTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CountingUnitType), request.Id);
        }

        _context.CountingUnitTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}