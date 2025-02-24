using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.CreateWidgetComment;
public record CreateWidgetCommentCommand : IRequest<int>
{
    public string? PostTitle { get; init; }
    public string? Text { get; init; }
    public Guid AuthorId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateWidgetCommentCommandHandler : IRequestHandler<CreateWidgetCommentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateWidgetCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateWidgetCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = new WidgetComment()
        {
            PostTitle = request.PostTitle,
            Text = request.Text,
            AuthorId = request.AuthorId,
            IsActive = request.IsActive,
        };


        _context.WidgetComments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
