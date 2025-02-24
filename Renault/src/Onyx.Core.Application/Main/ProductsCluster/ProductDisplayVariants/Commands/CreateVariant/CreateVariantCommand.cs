using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.CreateVariant;
public record CreateVariantCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public int ProductId { get; init; }
    public bool IsBestSeller { get; init; }
    public bool IsFeatured { get; init; }
    public bool IsLatest { get; init; }
    public bool IsNew { get; init; }
    public bool IsPopular { get; init; }
    public bool IsSale { get; init; }
    public bool IsSpecialOffer { get; init; }
    public bool IsTopRated { get; init; }
    public bool IsActive { get; init; }
}

public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateVariantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductDisplayVariant()
        {
            Name = request.Name,
            ProductId = request.ProductId,
            IsBestSeller = request.IsBestSeller,
            IsFeatured = request.IsFeatured,
            IsLatest = request.IsLatest,
            IsNew = request.IsNew,
            IsPopular = request.IsPopular,
            IsSale = request.IsSale,
            IsSpecialOffer = request.IsSpecialOffer,
            IsTopRated = request.IsTopRated,
            IsActive = request.IsActive
        };

        _context.ProductDisplayVariants.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
