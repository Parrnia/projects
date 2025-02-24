using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Commands.CreateFamily;
public record CreateFamilyCommand : IRequest<int>
{
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int VehicleBrandId { get; init; }
    public bool IsActive { get; init; }
}
public class CreateFamilyCommandHandler : IRequestHandler<CreateFamilyCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Family
        {
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            VehicleBrandId = request.VehicleBrandId,
            IsActive = request.IsActive,
        };

        _context.Families.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
