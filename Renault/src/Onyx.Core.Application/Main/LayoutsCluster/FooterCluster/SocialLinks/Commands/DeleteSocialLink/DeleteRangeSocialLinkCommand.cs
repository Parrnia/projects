using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.DeleteSocialLink;

public class DeleteRangeSocialLinkCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeSocialLinkCommandHandler : IRequestHandler<DeleteRangeSocialLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeSocialLinkCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeSocialLinkCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.SocialLinks
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(SocialLink), id);
            }

            _context.SocialLinks.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.Icon, cancellationToken);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
