using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.CreateCorporationInfo;
public record CreateCorporationInfoCommand : IRequest<int>
{
    public string ContactUsMessage { get; init; } = null!;
    public string PoweredBy { get; init; } = null!;
    public string CallUs { get; init; } = null!;
    public Guid DesktopLogo { get; init; }
    public Guid MobileLogo { get; init; }
    public Guid FooterLogo { get; init; }
    public Guid SliderBackGroundImage { get; init; }
    public bool IsDefault { get; set; }
}

public class CreateCorporationInfoCommandHandler : IRequestHandler<CreateCorporationInfoCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateCorporationInfoCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateCorporationInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = new CorporationInfo()
        {
            ContactUsMessage = request.ContactUsMessage,
            PoweredBy = request.PoweredBy,
            CallUs = request.CallUs,
            DesktopLogo = request.DesktopLogo,
            MobileLogo = request.MobileLogo,
            FooterLogo = request.FooterLogo,
            SliderBackGroundImage = request.SliderBackGroundImage,
            IsDefault = request.IsDefault
        };

        var corporationInfos = await _context.CorporationInfos.ToListAsync(cancellationToken);
        if (request.IsDefault)
        {
            corporationInfos?.ForEach(d => d.IsDefault = false);
        }
        if (corporationInfos != null && corporationInfos.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        await _fileStore.SaveStoredFile(
            request.DesktopLogo,
            FileCategory.CorporationInfo.ToString(),
            FileCategory.CorporationInfo,
            null,
            false,
            cancellationToken);


        await _fileStore.SaveStoredFile(
            request.MobileLogo,
            FileCategory.CorporationInfo.ToString(),
            FileCategory.CorporationInfo,
            null,
            false,
            cancellationToken);


        await _fileStore.SaveStoredFile(
            request.FooterLogo,
            FileCategory.CorporationInfo.ToString(),
            FileCategory.CorporationInfo,
            null,
            false,
            cancellationToken);


        await _fileStore.SaveStoredFile(
            request.SliderBackGroundImage,
            FileCategory.CorporationInfo.ToString(),
            FileCategory.CorporationInfo,
            null,
            false,
            cancellationToken);

        _context.CorporationInfos.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
