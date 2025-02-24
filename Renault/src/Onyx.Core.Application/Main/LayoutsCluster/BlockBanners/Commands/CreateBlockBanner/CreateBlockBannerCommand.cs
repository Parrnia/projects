using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.CreateBlockBanner;
public record CreateBlockBannerCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string Subtitle { get; init; } = null!;
    public string ButtonText { get; init; } = null!;
    public Guid Image { get; init; }
    public int BlockBannerPosition { get; init; }
    public bool IsActive { get; init; }
}

public class CreateBlockBannerCommandHandler : IRequestHandler<CreateBlockBannerCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;


    public CreateBlockBannerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateBlockBannerCommand request, CancellationToken cancellationToken)
    {
        var entity = new BlockBanner()
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            ButtonText = request.ButtonText,
            Image = request.Image,
            BlockBannerPosition = (BlockBannerPosition)request.BlockBannerPosition,
            IsActive = request.IsActive
        };

        await _fileStore.SaveStoredFile(
            request.Image,
            FileCategory.BlockBanner.ToString(),
            FileCategory.BlockBanner,
            null,
            false,
            cancellationToken);

        _context.BlockBanners.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
