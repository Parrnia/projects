using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.AddVehicleToCustomer;

public record AddVehicleToCustomerCommand : IRequest<Unit>
{
    public Guid CustomerId { get; set; }
    public int VehicleId { get; init; }
    public int KindId { get; init; }
}

public class AddVehicleToCustomerCommandHandler : IRequestHandler<AddVehicleToCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddVehicleToCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddVehicleToCustomerCommand request, CancellationToken cancellationToken)
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

        var vehicleEntity = await _context.Vehicles
            .FindAsync(new object[] { request.VehicleId }, cancellationToken);

        if (vehicleEntity == null)
        {
            throw new NotFoundException(nameof(Vehicle), request.VehicleId);
        }


        entity.Vehicles.Add(vehicleEntity);




        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}