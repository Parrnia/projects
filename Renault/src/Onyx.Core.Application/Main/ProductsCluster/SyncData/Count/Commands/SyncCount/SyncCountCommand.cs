using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.SyncData.Count.Commands.SyncCount;

public record SyncCountCommand : IRequest<bool>;

public class SyncCountCommandHandler : IRequestHandler<SyncCountCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;

    public SyncCountCommandHandler(IApplicationDbContext context, ISevenSoftService sevenSoftService)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
    }

    public async Task<bool> Handle(SyncCountCommand request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Where(c => c.Related7SoftProductId != null)
            .ToListAsync(cancellationToken);
        var productIds = products
            .Select(c => c.Related7SoftProductId ?? Guid.Empty).ToList();

        var syncCountsResult = await _sevenSoftService.SyncCounts(productIds, cancellationToken);

        if (!syncCountsResult.IsSuccessful || syncCountsResult.ProductCounts == null || !syncCountsResult.ProductCounts.Any())
        {
            return false;
        }

        foreach (var product in products)
        {
            var productAttributeOption = product.AttributeOptions.SingleOrDefault();
            var count = syncCountsResult.ProductCounts.SingleOrDefault(c => c.PartId == product.Related7SoftProductId)?.Number;
            productAttributeOption?.SetTotalCount(count ?? 0);
        }

        await _context.SaveChangesAsync(cancellationToken);


        return true;
    }
}