using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.UpdateBlockBanner;
public record UpdateBlockBannerCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Subtitle { get; init; } = null!;
    public string ButtonText { get; init; } = null!;
    public Guid Image { get; init; }
    public int BlockBannerPosition { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateBlockBannerCommandHandler : IRequestHandler<UpdateBlockBannerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateBlockBannerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateBlockBannerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockBanners
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(BlockBanner), request.Id);
        }

        entity.Title = request.Title;
        entity.Subtitle = request.Subtitle;
        entity.ButtonText = request.ButtonText;
        entity.BlockBannerPosition =(BlockBannerPosition) request.BlockBannerPosition;
        entity.IsActive = request.IsActive;

        if (entity.Image != request.Image)
        {
            await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.Image,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Image = request.Image;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
