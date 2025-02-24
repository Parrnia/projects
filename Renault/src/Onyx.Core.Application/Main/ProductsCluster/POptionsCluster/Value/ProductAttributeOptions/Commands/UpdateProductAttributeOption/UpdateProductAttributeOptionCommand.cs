using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.UpdateProductAttributeOption;
public record UpdateProductAttributeOptionCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int TotalCount { get; init; }
    public double SafetyStockQty { get; init; }
    public double MinStockQty { get; init; }
    public double MaxStockQty { get; init; }
    public double? MaxSalePriceNonCompanyProductPercent { get; init; }
    public IList<UpdateProductAttributeOptionValueCommand> ProductAttributeOptionValues { get; init; } = new List<UpdateProductAttributeOptionValueCommand>();
    public bool IsDefault { get; init; }
    public int ProductId { get; init; }
}
public record UpdateProductAttributeOptionValueCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
}

public class UpdateProductAttributeOptionCommandHandler : IRequestHandler<UpdateProductAttributeOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeOptions
            .Include(c => c.OptionValues)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.Id);
        }

        var productAttributeOptionValues = request.ProductAttributeOptionValues
            .Select(productAttributeOptionValueCommand => new ProductAttributeOptionValue { Name = productAttributeOptionValueCommand.Name, Value = productAttributeOptionValueCommand.Value }).ToList();


        entity.SetTotalCount(request.TotalCount);
        entity.SafetyStockQty = request.SafetyStockQty;
        entity.MinStockQty = request.MinStockQty;
        entity.MaxStockQty = request.MaxStockQty;
        entity.MaxSalePriceNonCompanyProductPercent = request.MaxSalePriceNonCompanyProductPercent;
        entity.IsDefault = request.IsDefault;
        entity.ProductId = request.ProductId;
        entity.OptionValues = productAttributeOptionValues;


        var productAttributeOptions = await _context.ProductAttributeOptions.Where(c => c.ProductId == request.ProductId).ToListAsync(cancellationToken);
        if (request.IsDefault)
        {
            productAttributeOptions?.ForEach(d => d.IsDefault = false);
        }
        if (productAttributeOptions != null && productAttributeOptions.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}