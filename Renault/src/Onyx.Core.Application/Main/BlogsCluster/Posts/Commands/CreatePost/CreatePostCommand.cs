using MediatR;
using Microsoft.AspNetCore.Http;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.Posts.Commands.CreatePost;
public record CreatePostCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string Body { get; init; } = null!;
    public IFormFile? Image { get; init; }
    public int BlogCategoryId { get; init; }
    public Guid AuthorId { get; set; }
    public bool IsActive { get; init; }
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = new Post()
        {
            Title = request.Title,
            Body = request.Body,
            Image = await request.Image.ToByteArrayAsync(),
            BlogCategoryId = request.BlogCategoryId,
            AuthorId = request.AuthorId,
            IsActive = request.IsActive,
        };

        _context.Posts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
