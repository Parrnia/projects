using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.DeleteProductCategory;

public record DeleteProductCategoryCommand(int Id) : IRequest<Unit>;

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteProductCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductCategory), request.Id);
        }
        _context.ProductCategories.Remove(entity);

        if (entity.Image != null)
        {
            await _fileStore.RemoveStoredFile((Guid)entity.Image, cancellationToken);
        }

        var link = await _context.Links
            .SingleOrDefaultAsync(c => c.RelatedProductCategoryId == request.Id, cancellationToken);
        if (link != null)
        {
            _context.Links.Remove(link);
            if (entity.MenuImage != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.MenuImage, cancellationToken);
            }
        }

        

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}