using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.UpdateCorporationInfo;
public record UpdateCorporationInfoCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string ContactUsMessage { get; init; } = null!;
    public string PoweredBy { get; init; } = null!;
    public string CallUs { get; init; } = null!;
    public Guid DesktopLogo { get; init; }
    public Guid MobileLogo { get; init; }
    public Guid FooterLogo { get; init; }
    public Guid SliderBackGroundImage { get; init; }
    public bool IsDefault { get; set; }
}
public class UpdateCorporationInfoCommandHandler : IRequestHandler<UpdateCorporationInfoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateCorporationInfoCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateCorporationInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.Id);
        }

        entity.ContactUsMessage = request.ContactUsMessage;
        entity.PoweredBy = request.PoweredBy;
        entity.CallUs = request.CallUs;
        entity.IsDefault = request.IsDefault;

        var corporationInfos = _context.CorporationInfos.ToList();
        if (request.IsDefault)
        {
            corporationInfos?.ForEach(d => d.IsDefault = false);
        }
        if (corporationInfos != null && corporationInfos.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        if (entity.DesktopLogo != request.DesktopLogo)
        {
            await _fileStore.RemoveStoredFile(entity.DesktopLogo, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.DesktopLogo,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.DesktopLogo = request.DesktopLogo;
        }
        if (entity.MobileLogo != request.MobileLogo)
        {
            await _fileStore.RemoveStoredFile(entity.MobileLogo, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.MobileLogo,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.MobileLogo = request.MobileLogo;
        }
        if (entity.FooterLogo != request.FooterLogo)
        {
            await _fileStore.RemoveStoredFile(entity.FooterLogo, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.FooterLogo,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.FooterLogo = request.FooterLogo;
        }
        if (entity.SliderBackGroundImage != request.SliderBackGroundImage)
        {
            await _fileStore.RemoveStoredFile(entity.SliderBackGroundImage, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.SliderBackGroundImage,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.SliderBackGroundImage = request.SliderBackGroundImage;
        }
        

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}