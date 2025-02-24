using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Commands.DeleteRangeProductImages;

public class DeleteRangeProductImagesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}
public class DeleteRangeProductImagesCommandHandler : IRequestHandler<DeleteRangeProductImagesCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeProductImagesCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeProductImagesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductImages
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductImage), id);
            }

            _context.ProductImages.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
