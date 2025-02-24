using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.DeleteProductBrand;

public class DeleteRangeProductBrandCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeProductBrandCommandHandler : IRequestHandler<DeleteRangeProductBrandCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeProductBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeProductBrandCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductBrands
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductBrand), id);
            }

            _context.ProductBrands.Remove(entity);
            if (entity.BrandLogo != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.BrandLogo, cancellationToken);
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
