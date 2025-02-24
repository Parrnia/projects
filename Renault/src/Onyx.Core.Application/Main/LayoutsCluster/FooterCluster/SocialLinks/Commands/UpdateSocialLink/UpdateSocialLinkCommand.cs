using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.UpdateSocialLink;
public record UpdateSocialLinkCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Url { get; init; } = null!;
    public Guid Icon { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateSocialLinkCommandHandler : IRequestHandler<UpdateSocialLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateSocialLinkCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateSocialLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SocialLinks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(SocialLink), request.Id);
        }

        entity.Url = request.Url;
        entity.IsActive = request.IsActive;

        if (entity.Icon != request.Icon)
        {
            await _fileStore.RemoveStoredFile(entity.Icon, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.Icon,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Icon = request.Icon;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}