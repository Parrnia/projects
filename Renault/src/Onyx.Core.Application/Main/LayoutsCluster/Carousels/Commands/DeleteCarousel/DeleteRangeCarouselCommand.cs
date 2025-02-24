using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.DeleteCarousel;

public class DeleteRangeCarouselCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeCarouselCommandHandler : IRequestHandler<DeleteRangeCarouselCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeCarouselCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeCarouselCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Carousels
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Carousel), id);
            }

            _context.Carousels.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.DesktopImage, cancellationToken);
            await _fileStore.RemoveStoredFile(entity.MobileImage, cancellationToken);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
