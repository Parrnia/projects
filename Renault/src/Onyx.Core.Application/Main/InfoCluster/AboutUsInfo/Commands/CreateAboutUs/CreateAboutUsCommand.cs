using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.CreateAboutUs;
public record CreateAboutUsCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string TextContent { get; init; } = null!;
    public Guid ImageContent { get; init; }
    public bool IsDefault { get; set; }
}

public class CreateAboutUsCommandHandler : IRequestHandler<CreateAboutUsCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateAboutUsCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateAboutUsCommand request, CancellationToken cancellationToken)
    {
        var entity = new AboutUs()
        {
            Title = request.Title,
            TextContent = request.TextContent,
            ImageContent = request.ImageContent,
            IsDefault = request.IsDefault
        };

        var aboutUsEnumerable = _context.AboutUsEnumerable.ToList();
        if (request.IsDefault)
        {
            aboutUsEnumerable?.ForEach(d => d.IsDefault = false);
        }
        if (aboutUsEnumerable != null && aboutUsEnumerable.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        await _fileStore.SaveStoredFile(
            request.ImageContent,
            FileCategory.AboutUs.ToString(),
            FileCategory.AboutUs,
            null,
            false,
            cancellationToken);

        _context.AboutUsEnumerable.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
