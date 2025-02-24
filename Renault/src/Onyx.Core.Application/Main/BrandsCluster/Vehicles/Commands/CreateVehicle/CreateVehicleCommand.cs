using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Commands.CreateVehicle;
public record CreateVehicleCommand : IRequest<int>
{
    public string? VinNumber { get; init; }
    public int KindId { get; init; }
    public Guid CustomerId { get; set; }
}

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicleEntity = new Vehicle()
        {
            VinNumber = request.VinNumber,
            KindId = request.KindId,
            CustomerId = request.CustomerId
        };

        _context.Vehicles.Add(vehicleEntity);

        await _context.SaveChangesAsync(cancellationToken);

        return vehicleEntity.Id;
    }
}
