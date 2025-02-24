using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.DeleteBlockBanner;

public class DeleteRangeBlockBannerCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeBlockBannerCommandHandler : IRequestHandler<DeleteRangeBlockBannerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeBlockBannerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeBlockBannerCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.BlockBanners
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BlockBanner), id);
            }

            _context.BlockBanners.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
