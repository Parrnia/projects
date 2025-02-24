using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.DeleteBlockBanner;
public record DeleteBlockBannerCommand(int Id) : IRequest<Unit>;

public class DeleteBlockBannerCommandHandler : IRequestHandler<DeleteBlockBannerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteBlockBannerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteBlockBannerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockBanners
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(BlockBanner), request.Id);
        }

        _context.BlockBanners.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}