using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.DeleteAboutUs;

public record DeleteAboutUsCommand(int Id) : IRequest<Unit>;

public class DeleteAboutUsCommandHandler : IRequestHandler<DeleteAboutUsCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteAboutUsCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteAboutUsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AboutUsEnumerable
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.InfoCluster.AboutUs), request.Id);
        }

        _context.AboutUsEnumerable.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.ImageContent, cancellationToken);
            

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}