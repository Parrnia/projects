using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductKinds.Commands;


public record CreateProductKindsCommand : IRequest<int>
{
    public int KindId { get; set; }
    public int ProductId { get; set; }
}

public class CreateProductKindsCommandHandler : IRequestHandler<CreateProductKindsCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductKindsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductKindsCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
                          .Include(c => c.ProductDisplayVariants)
                          .Include(c => c.ProductBrand)
                          .SingleOrDefaultAsync(c => c.Id == request.ProductId, cancellationToken)
                      ??
                      throw new NotFoundException(nameof(Product), request.ProductId);
        var kind = await _context.Kinds
                       .Include(c => c.Model)
                       .ThenInclude(c => c.Family)
                       .SingleOrDefaultAsync(c => c.Id == request.KindId, cancellationToken)
                   ??
                   throw new NotFoundException(nameof(Kind), request.KindId);
        
        var entity = new ProductKind()
        {
            KindId=request.KindId,
            ProductId = request.ProductId
        };


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


        _context.ProductKinds.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
