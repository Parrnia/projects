using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Validators;

public record UniqueVehicleVinNumberValidator : IRequest<bool>
{
    public int VehicleId { get; init; }
    public string VinNumber { get; init; } = null!;
}

public class UniqueVehicleVinNumberValidatorHandler : IRequestHandler<UniqueVehicleVinNumberValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueVehicleVinNumberValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueVehicleVinNumberValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Vehicles.Where(c => c.Id != request.VehicleId)
            .AllAsync(e => e.VinNumber != request.VinNumber, cancellationToken);
        return isUnique;
    }
}
    