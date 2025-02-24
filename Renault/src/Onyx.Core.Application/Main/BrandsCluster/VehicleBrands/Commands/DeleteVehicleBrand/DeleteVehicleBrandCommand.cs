using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.DeleteVehicleBrand;

public record DeleteVehicleBrandCommand(int Id) : IRequest<Unit>;

public class DeleteVehicleBrandCommandHandler : IRequestHandler<DeleteVehicleBrandCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteVehicleBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VehicleBrands
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(VehicleBrand), request.Id);
        }

        _context.VehicleBrands.Remove(entity);

        if (entity.BrandLogo != null)
        {
            await _fileStore.RemoveStoredFile((Guid)entity.BrandLogo, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}