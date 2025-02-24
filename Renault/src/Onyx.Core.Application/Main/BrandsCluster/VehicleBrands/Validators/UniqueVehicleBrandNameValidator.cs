using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Validators;

public record UniqueVehicleBrandNameValidator : IRequest<bool>
{
    public int VehicleBrandId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueVehicleBrandNameValidatorHandler : IRequestHandler<UniqueVehicleBrandNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueVehicleBrandNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueVehicleBrandNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.VehicleBrands.Where(c => c.Id != request.VehicleBrandId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    