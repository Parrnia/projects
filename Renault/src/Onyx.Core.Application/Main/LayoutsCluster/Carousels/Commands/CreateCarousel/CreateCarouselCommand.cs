using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.CreateCarousel;
public record CreateCarouselCommand : IRequest<int>
{
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

public class CreateCarouselCommandHandler : IRequestHandler<CreateCarouselCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateCarouselCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateCarouselCommand request, CancellationToken cancellationToken)
    {
        var entity = new Carousel()
        {
            Url = request.Url,
            DesktopImage = request.DesktopImage,
            MobileImage = request.MobileImage,
            Offer = request.Offer,
            Title = request.Title,
            Details = request.Details,
            ButtonLabel =  request.ButtonLabel,
            Order = request.Order,
            IsActive = request.IsActive
        };

        await _fileStore.SaveStoredFile(
            request.DesktopImage,
            FileCategory.Carousel.ToString(),
            FileCategory.Carousel,
            null,
            false,
            cancellationToken);

        await _fileStore.SaveStoredFile(
            request.MobileImage,
            FileCategory.Carousel.ToString(),
            FileCategory.Carousel,
            null,
            false,
            cancellationToken);

        _context.Carousels.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
