using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(int Id, Guid? CustomerId) : IRequest<Unit>;

public class DeleteVinCommandHandler : IRequestHandler<DeleteVehicleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteVinCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vehicles
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Vehicle), request.Id);
        }
        if (request.CustomerId != null && entity.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("DeleteVehicleCommand");
        }
        _context.Vehicles.Remove(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}