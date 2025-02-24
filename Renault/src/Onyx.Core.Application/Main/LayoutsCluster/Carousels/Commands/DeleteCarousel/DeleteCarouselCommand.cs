using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.DeleteCarousel;
public record DeleteCarouselCommand(int Id) : IRequest<Unit>;

public class DeleteCarouselCommandHandler : IRequestHandler<DeleteCarouselCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteCarouselCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteCarouselCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Carousels
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Carousel), request.Id);
        }

        _context.Carousels.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.DesktopImage, cancellationToken);
        await _fileStore.RemoveStoredFile(entity.MobileImage, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}