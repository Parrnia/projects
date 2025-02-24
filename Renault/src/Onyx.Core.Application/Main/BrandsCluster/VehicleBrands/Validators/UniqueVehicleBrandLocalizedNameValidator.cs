using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Validators;

public record UniqueVehicleBrandLocalizedNameValidator : IRequest<bool>
{
    public int VehicleBrandId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueVehicleBrandLocalizedNameValidatorHandler : IRequestHandler<UniqueVehicleBrandLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueVehicleBrandLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueVehicleBrandLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.VehicleBrands.Where(c => c.Id != request.VehicleBrandId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
