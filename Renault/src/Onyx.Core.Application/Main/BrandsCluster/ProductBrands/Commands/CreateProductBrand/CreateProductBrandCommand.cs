using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.CreateProductBrand;
public record CreateProductBrandCommand : IRequest<int>
{
    public Guid? BrandLogo { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int Code { get; init; }
    public int? CountryId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateProductBrandCommandHandler : IRequestHandler<CreateProductBrandCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateProductBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateProductBrandCommand request, CancellationToken cancellationToken)
    {

        var entity = new ProductBrand()
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
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
        }

        _context.ProductBrands.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
