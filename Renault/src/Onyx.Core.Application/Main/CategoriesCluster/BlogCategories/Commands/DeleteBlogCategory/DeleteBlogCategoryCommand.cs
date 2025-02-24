using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.DeleteBlogCategory;

public record DeleteBlogCategoryCommand(int Id) : IRequest<Unit>;

public class DeleteBlogCategoryCommandHandler : IRequestHandler<DeleteBlogCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteBlogCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlogCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(BlogCategory), request.Id);
        }

        _context.BlogCategories.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}