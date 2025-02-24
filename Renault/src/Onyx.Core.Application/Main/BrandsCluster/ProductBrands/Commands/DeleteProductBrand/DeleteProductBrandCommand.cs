using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.DeleteProductBrand;

public record DeleteProductBrandCommand(int Id) : IRequest<Unit>;

public class DeleteProductBrandCommandHandler : IRequestHandler<DeleteProductBrandCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteProductBrandCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteProductBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductBrands
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductBrand), request.Id);
        }

        _context.ProductBrands.Remove(entity);

        if (entity.BrandLogo != null)
        {
            await _fileStore.RemoveStoredFile((Guid) entity.BrandLogo, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}