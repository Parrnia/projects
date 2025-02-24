using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.DeleteProductCategory;

public class DeleteRangeProductCategoryCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeProductCategoryCommandHandler : IRequestHandler<DeleteRangeProductCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeProductCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeProductCategoryCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductCategories
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductCategory), id);
            }

            _context.ProductCategories.Remove(entity);
            if (entity.Image != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.Image, cancellationToken);
            }
            var link = await _context.Links
                .SingleOrDefaultAsync(c => c.RelatedProductCategoryId == id, cancellationToken);
            if (link != null)
            {
                _context.Links.Remove(link);
                if (entity.MenuImage != null)
                {
                    await _fileStore.RemoveStoredFile((Guid)entity.MenuImage, cancellationToken);
                }
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
