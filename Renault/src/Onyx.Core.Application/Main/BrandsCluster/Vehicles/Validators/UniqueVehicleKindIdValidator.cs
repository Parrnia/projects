using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Validators;

public record UniqueVehicleKindIdValidator : IRequest<bool>
{
    public int VehicleId { get; init; }
    public int KindId { get; init; }
}

public class UniqueVehicleKindIdValidatorHandler : IRequestHandler<UniqueVehicleKindIdValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueVehicleKindIdValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueVehicleKindIdValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Vehicles.Where(c => c.Id != request.VehicleId)
            .AllAsync(e => e.KindId != request.KindId, cancellationToken);
        return isUnique;
    }
}
    