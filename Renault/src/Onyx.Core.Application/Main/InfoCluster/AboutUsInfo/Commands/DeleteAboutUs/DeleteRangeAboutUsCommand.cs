using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.DeleteAboutUs;

public class DeleteRangeAboutUsCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeAboutUsCommandHandler : IRequestHandler<DeleteRangeAboutUsCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeAboutUsCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeAboutUsCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.AboutUsEnumerable
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(AboutUs), id);
            }

            _context.AboutUsEnumerable.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.ImageContent, cancellationToken);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
