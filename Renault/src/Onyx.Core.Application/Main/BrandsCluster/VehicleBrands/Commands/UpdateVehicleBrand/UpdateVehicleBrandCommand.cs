using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.UpdateVehicleBrand;
public record UpdateVehicleBrandCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public Guid? BrandLogo { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int Code { get; init; }
    public int? CountryId { get; init; }
    public bool IsActive { get; init; }

}

public class UpdateVehicleBrandCommandHandler : IRequestHandler<UpdateVehicleBrandCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateVehicleBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VehicleBrands
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(VehicleBrand), request.Id);
        }

        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.Slug = request.LocalizedName.ToLower().Replace(' ', '-');
        entity.CountryId = request.CountryId;
        entity.IsActive = request.IsActive;

        if (entity.BrandLogo != request.BrandLogo)
        {
            if (entity.BrandLogo != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.BrandLogo, cancellationToken);
            }

            if (request.BrandLogo != null)
            {
                await _fileStore.SaveStoredFile(
                    (Guid)request.BrandLogo,
                    FileCategory.ProductBrand.ToString(),
                    FileCategory.ProductBrand,
                    null,
                    false,
                    cancellationToken);
            }
            entity.BrandLogo = request.BrandLogo;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}