using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.DeleteCorporationInfo;

public class DeleteRangeCorporationInfoCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeCorporationInfoCommandHandler : IRequestHandler<DeleteRangeCorporationInfoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeCorporationInfoCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeCorporationInfoCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.CorporationInfos
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(CorporationInfo), id);
            }

            _context.CorporationInfos.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.DesktopLogo, cancellationToken);
            await _fileStore.RemoveStoredFile(entity.MobileLogo, cancellationToken);
            await _fileStore.RemoveStoredFile(entity.FooterLogo, cancellationToken);
            await _fileStore.RemoveStoredFile(entity.SliderBackGroundImage, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
