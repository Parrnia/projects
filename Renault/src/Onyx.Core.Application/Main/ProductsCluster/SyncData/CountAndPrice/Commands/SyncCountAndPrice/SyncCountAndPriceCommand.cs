using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.SyncData.CountAndPrice.Commands.SyncCountAndPrice;

public record SyncCountAndPriceCommand : IRequest<bool>;

public class SyncCountAndPriceCommandHandler : IRequestHandler<SyncCountAndPriceCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;

    public SyncCountAndPriceCommandHandler(IApplicationDbContext context,
        ISevenSoftService sevenSoftService)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
    }

    public async Task<bool> Handle(SyncCountAndPriceCommand request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.Prices)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Where(c => c.Related7SoftProductId != null)
            .ToListAsync(cancellationToken);
        var productIds = products
            .Select(c => c.Related7SoftProductId ?? Guid.Empty).ToList();
        var syncCountsResult = await _sevenSoftService.SyncCounts(productIds, cancellationToken);
        var syncPricesResult = await _sevenSoftService.SyncPrices(productIds, cancellationToken);

        if (!syncCountsResult.IsSuccessful
            || syncCountsResult.ProductCounts == null
            || !syncCountsResult.ProductCounts.Any()
            || !syncPricesResult.IsSuccessful
            || syncPricesResult.ProductPrices == null
            || !syncPricesResult.ProductPrices.Any())
        {
            return false;
        }

        foreach (var product in products)
        {
            var productAttributeOption = product.AttributeOptions.SingleOrDefault();
            var count = syncCountsResult.ProductCounts.SingleOrDefault(c => c.PartId == product.Related7SoftProductId)?.Number;
            productAttributeOption?.SetTotalCount(count ?? 0);
            var price = syncPricesResult.ProductPrices.SingleOrDefault(c => c.PartId == product.Related7SoftProductId)?.Price;
            productAttributeOption?.SetPrice(price ?? 0);
        }

        await _context.SaveChangesAsync(cancellationToken);


        return true;
    }
}