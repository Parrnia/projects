using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.UpdateAboutUs;
public record UpdateAboutUsCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string TextContent { get; init; } = null!;
    public Guid ImageContent { get; init; }
    public bool IsDefault { get; set; }
}

public class UpdateAboutUsCommandHandler : IRequestHandler<UpdateAboutUsCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateAboutUsCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateAboutUsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AboutUsEnumerable
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AboutUs), request.Id);
        }

        entity.TextContent = request.TextContent;
        entity.IsDefault = request.IsDefault;
        entity.Title = request.Title;

        var aboutUsEnumerable = _context.AboutUsEnumerable.ToList();
        if (request.IsDefault)
        {
            aboutUsEnumerable?.ForEach(d => d.IsDefault = false);
        }
        if (aboutUsEnumerable != null && aboutUsEnumerable.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        if (entity.ImageContent != request.ImageContent)
        {
            await _fileStore.RemoveStoredFile(entity.ImageContent, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.ImageContent,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.ImageContent = request.ImageContent;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}