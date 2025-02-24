using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.CreateVehicleBrand;
public record CreateVehicleBrandCommand : IRequest<int>
{
    public Guid? BrandLogo { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int Code { get; init; }
    public int? CountryId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateVehicleBrandCommandHandler : IRequestHandler<CreateVehicleBrandCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateVehicleBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new VehicleBrand()
        {
            BrandLogo = request.BrandLogo,
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            Code = request.Code,
            Slug = request.LocalizedName.ToLower().Replace(' ', '-'),
            CountryId = request.CountryId,
            IsActive = request.IsActive,
        };

        if (request.BrandLogo != null)
        {
            await _fileStore.SaveStoredFile(
                (Guid)request.BrandLogo,
                FileCategory.VehicleBrand.ToString(),
                FileCategory.VehicleBrand,
                null,
                false,
                cancellationToken);
        }


        _context.VehicleBrands.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
