using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.RemoveVehicleFromCustomer;

public record RemoveVehicleFromCustomerCommand(Guid CustomerId, int VehicleId) : IRequest<Unit>;

public class RemoveVehicleFromCustomerCommandHandler : IRequestHandler<RemoveVehicleFromCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RemoveVehicleFromCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveVehicleFromCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.Include(c => c.Vehicles)
            .SingleOrDefaultAsync(e => e.Id == request.CustomerId, cancellationToken);

        if (entity == null)
        {
            var recoverEntity = new Customer()
            {
                Id = request.CustomerId
            };

            _context.Customers.Add(recoverEntity);
            await _context.SaveChangesAsync(cancellationToken);
            entity = recoverEntity;
        }

        var vehicleEntity = entity.Vehicles.SingleOrDefault(c => c.Id == request.VehicleId);

        if (vehicleEntity == null)
        {
            throw new NotFoundException(nameof(Vehicle), request.VehicleId);
        }

        entity.Vehicles.Remove(vehicleEntity);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}