using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.UpdateWidgetComment;
public record UpdateWidgetCommentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string? PostTitle { get; init; }
    public string? Text { get; init; }
    public Guid? AuthorId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateWidgetCommentCommandHandler : IRequestHandler<UpdateWidgetCommentCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateWidgetCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateWidgetCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WidgetComments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WidgetComment), request.Id);
        }

        entity.PostTitle = request.PostTitle;
        entity.Text = request.Text;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}