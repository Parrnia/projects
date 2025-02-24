using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.UpdateVariant;
public record UpdateVariantCommand : IRequest<Unit>
{
    public int Id { get; init; }
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

public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateVariantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductDisplayVariants
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductDisplayVariant), request.Id);
        }

        entity.Name = request.Name;
        entity.ProductId = request.ProductId;
        entity.IsBestSeller = request.IsBestSeller;
        entity.IsFeatured = request.IsFeatured;
        entity.IsLatest = request.IsLatest;
        entity.IsNew = request.IsNew;
        entity.IsPopular = request.IsPopular;
        entity.IsSale = request.IsSale;
        entity.IsSpecialOffer = request.IsSpecialOffer;
        entity.IsTopRated = request.IsTopRated;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}