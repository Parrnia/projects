using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
public record CreateCommentCommand : IRequest<int>
{
    public string Text { get; init; } = null!;
    public Guid AuthorId { get; set; }
    public int PostId { get; init; }
    public int? ParentCommentId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Comment()
        {
            Text = request.Text,
            AuthorId = request.AuthorId,
            PostId = request.PostId,
            IsActive = request.IsActive
            //ParentCommentId = request.ParentCommentId
        };

        _context.Comments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
