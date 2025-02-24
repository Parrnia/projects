using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Commands.UpdateVehicle;
public record UpdateVehicleCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string? VinNumber { get; init; }
    public int KindId { get; init; }
    public Guid CustomerId { get; init; }
}

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vehicles
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Vehicle), request.Id);
        }


        entity.VinNumber = request.VinNumber;
        entity.KindId = request.KindId;
        entity.CustomerId = request.CustomerId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}