using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.DeleteVehicleBrand;

public class DeleteRangeVehicleBrandCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeVehicleBrandCommandHandler : IRequestHandler<DeleteRangeVehicleBrandCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeVehicleBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeVehicleBrandCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.VehicleBrands
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(VehicleBrand), id);
            }

            _context.VehicleBrands.Remove(entity);
            if (entity.BrandLogo != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.BrandLogo, cancellationToken);
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
