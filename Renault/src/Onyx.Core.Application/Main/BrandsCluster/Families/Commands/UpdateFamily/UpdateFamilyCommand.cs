using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Commands.UpdateFamily;
public record UpdateFamilyCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int VehicleBrandId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateFamilyCommandHandler : IRequestHandler<UpdateFamilyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Families
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Family), request.Id);
        }

        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.VehicleBrandId = request.VehicleBrandId;
        entity.IsActive = request.IsActive;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
