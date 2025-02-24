using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.UpdateCarousel;
public record UpdateCarouselCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Url { get; init; } = null!;
    public Guid DesktopImage { get; init; }
    public Guid MobileImage { get; init; }
    public string? Offer { get; init; }
    public string Title { get; init; } = null!;
    public string Details { get; init; } = null!;
    public string ButtonLabel { get; init; } = null!;
    public int Order { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateCarouselCommandHandler : IRequestHandler<UpdateCarouselCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateCarouselCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateCarouselCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Carousels
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Carousel), request.Id);
        }

        entity.Url = request.Url;
        entity.Offer = request.Offer;
        entity.Title = request.Title;
        entity.Details = request.Details;
        entity.ButtonLabel = request.ButtonLabel;
        entity.Order = request.Order;
        entity.IsActive = request.IsActive;

        if (entity.DesktopImage != request.DesktopImage)
        {
            await _fileStore.RemoveStoredFile(entity.DesktopImage, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.DesktopImage,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.DesktopImage = request.DesktopImage;
        }

        if (entity.MobileImage != request.MobileImage)
        {
            await _fileStore.RemoveStoredFile(entity.MobileImage, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.MobileImage,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.MobileImage = request.MobileImage;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
