using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Commands.UpdateKind;
public record UpdateKindCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int ModelId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateKindCommandHandler : IRequestHandler<UpdateKindCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateKindCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateKindCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Kinds
            .Include(c => c.Products)
            .ThenInclude(c => c.ProductBrand)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Kind), request.Id);
        }

        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;


        var dbModel = await _context.Models
                        .Include(c => c.Family)
                        .SingleOrDefaultAsync(c => c.Id == entity.ModelId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Model), entity.ModelId);

        var model = await _context.Models
                          .Include(c => c.Family)
                          .SingleOrDefaultAsync(c => c.Id == request.ModelId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Model), request.ModelId);

        var productDisplayVariants = await _context.ProductDisplayVariants.ToListAsync(cancellationToken);

        foreach (var product in entity.Products)
        {
            var productDisplayVariant = productDisplayVariants.SingleOrDefault(c =>
                c.ProductId == product.Id && c.Name == product.ProductBrand.LocalizedName + " " +
                dbModel.Family.LocalizedName + " " + product.LocalizedName);
            if (productDisplayVariant != null)
            {
                product.ProductDisplayVariants.Remove(productDisplayVariant);
            }
        }

        foreach (var product in entity.Products)
        {
            var productDisplayVariantName = product.ProductBrand.LocalizedName + " " +
                                            model.Family.LocalizedName + " " + product.LocalizedName;
            var dbProductDisplayVariant = productDisplayVariants.SingleOrDefault(c =>
                c.ProductId == product.Id && c.Name == productDisplayVariantName);
            if (dbProductDisplayVariant == null)
            {
                var productDisplayVariant = new ProductDisplayVariant()
                {
                    Name = productDisplayVariantName,
                    ProductId = product.Id
                };
                product.ProductDisplayVariants.Add(productDisplayVariant);
            }
        }

        entity.ModelId = request.ModelId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
