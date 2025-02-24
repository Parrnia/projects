using MediatR;
using Microsoft.AspNetCore.Http;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.Posts.Commands.UpdatePost;
public record UpdatePostCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Body { get; init; } = null!;
    public IFormFile? Image { get; init; }
    public int BlogCategoryId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        entity.Title = request.Title;
        entity.Body = request.Body;
        entity.Image = await request.Image.ToByteArrayAsync();
        entity.BlogCategoryId = request.BlogCategoryId;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}