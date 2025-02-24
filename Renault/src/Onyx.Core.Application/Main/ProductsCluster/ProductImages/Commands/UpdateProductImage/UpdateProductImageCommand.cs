using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Commands.UpdateProductImage;
public record UpdateProductImageCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public Guid Image { get; init; }
    public int Order { get; init; }
    public int ProductId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateProductImageCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductImages
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductImage), request.Id);
        }

        entity.Order = request.Order;
        entity.ProductId = request.ProductId;
        entity.IsActive = request.IsActive;

        if (entity.Image != request.Image)
        {
            await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.Image,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Image = request.Image;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}