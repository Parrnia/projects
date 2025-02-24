using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Commands.CreateProductImage;
public record CreateProductImageCommand : IRequest<int>
{
    public Guid Image { get; init; }
    public int Order { get; init; }
    public int ProductId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateProductImageCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductImage()
        {
            Image = request.Image,
            Order = request.Order,
            ProductId = request.ProductId,
            IsActive = request.IsActive
        };


        await _fileStore.SaveStoredFile(
            request.Image,
            FileCategory.ProductImage.ToString(),
            FileCategory.ProductImage,
            null,
            false,
            cancellationToken);

        _context.ProductImages.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
