using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.DeleteSocialLink;

public record DeleteSocialLinkCommand(int Id) : IRequest<Unit>;

public class DeleteSocialLinkCommandHandler : IRequestHandler<DeleteSocialLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteSocialLinkCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteSocialLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SocialLinks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(SocialLink), request.Id);
        }

        _context.SocialLinks.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.Icon, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}