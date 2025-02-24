using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.Posts.Commands.DeletePost;

public record DeletePostCommand(int Id, Guid? AuthorId) : IRequest<Unit>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }
        if (request.AuthorId != null && entity.AuthorId != request.AuthorId)
        {
            throw new ForbiddenAccessException("DeletePostCommand");
        }

        _context.Posts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}