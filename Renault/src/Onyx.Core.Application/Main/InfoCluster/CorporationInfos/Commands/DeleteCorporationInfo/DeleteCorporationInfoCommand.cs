using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.DeleteCorporationInfo;

public record DeleteCorporationInfoCommand(int Id) : IRequest<Unit>;

public class DeleteCorporationInfoCommandHandler : IRequestHandler<DeleteCorporationInfoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteCorporationInfoCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteCorporationInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.Id);
        }

        _context.CorporationInfos.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.DesktopLogo, cancellationToken);
        await _fileStore.RemoveStoredFile(entity.MobileLogo, cancellationToken);
        await _fileStore.RemoveStoredFile(entity.FooterLogo, cancellationToken);
        await _fileStore.RemoveStoredFile(entity.SliderBackGroundImage, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}