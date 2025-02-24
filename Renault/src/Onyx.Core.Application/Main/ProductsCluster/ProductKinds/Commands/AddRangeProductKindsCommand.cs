using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductKinds.Commands;


public record AddRangeProductKindsCommand : IRequest<List<int>>
{
    public List<int> KindIds { get; set; } = new List<int>();
    public int ProductId { get; set; }
}

public class AddRangeProductKindsCommandHandler : IRequestHandler<AddRangeProductKindsCommand, List<int>>
{
    private readonly IApplicationDbContext _context;

    public AddRangeProductKindsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> Handle(AddRangeProductKindsCommand request, CancellationToken cancellationToken)
    {
        var productKinds = new List<ProductKind>();
        var product = await _context.Products
                          .Include(c => c.ProductDisplayVariants)
                          .Include(c => c.ProductBrand)
                          .SingleOrDefaultAsync(c => c.Id == request.ProductId, cancellationToken)
                      ??
                      throw new NotFoundException(nameof(Product), request.ProductId);

        foreach (var kindId in request.KindIds)
        {
            var kind = await _context.Kinds
                .Include(c => c.Model)
                .ThenInclude(c => c.Family)
                .SingleOrDefaultAsync(c => c.Id == kindId, cancellationToken)
                       ??
                       throw new NotFoundException(nameof(Kind), kindId);

            var dbEntity =
                await _context.ProductKinds
                    .SingleOrDefaultAsync(c =>
                        c.ProductId == request.ProductId
                        && c.KindId == kindId, cancellationToken);

            if (dbEntity == null)
            {
                var entity = new ProductKind()
                {
                    KindId = kindId,
                    ProductId = request.ProductId
                };
                productKinds.Add(entity);


                var productDisplayVariantName = product.ProductBrand.LocalizedName + " " + kind.Model.Family.LocalizedName + " " + product.LocalizedName;
                var dbProductDisplayVariant = product.ProductDisplayVariants.SingleOrDefault(c =>
                    c.ProductId == product.Id && c.Name == productDisplayVariantName);
                if (dbProductDisplayVariant == null)
                {
                    var productDisplayVariant = new ProductDisplayVariant()
                    {
                        Name = productDisplayVariantName,
                    };
                    product.ProductDisplayVariants.Add(productDisplayVariant);
                }
            }
        }


        _context.ProductKinds.AddRange(productKinds);

        await _context.SaveChangesAsync(cancellationToken);

        return productKinds.Select(c => c.Id).ToList();
    }
}
