using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.DeleteWidgetComment;

public record DeleteWidgetCommentCommand(int Id, Guid? AuthorId) : IRequest<Unit>;

public class DeleteWidgetCommentCommandHandler : IRequestHandler<DeleteWidgetCommentCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteWidgetCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteWidgetCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WidgetComments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WidgetComment), request.Id);
        }
        if (request.AuthorId != null && entity.AuthorId != request.AuthorId)
        {
            throw new ForbiddenAccessException("DeleteWidgetCommentCommand");
        }

        _context.WidgetComments.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}