using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.CreateProductAttributeOptionValue;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.CreateProductAttributeOption;
public record CreateProductAttributeOptionCommand : IRequest<int>
{
    public double TotalCount { get; init; }
    public double SafetyStockQty { get; init; }
    public double MinStockQty { get; init; }
    public double MaxStockQty { get; init; }
    public double? MaxSalePriceNonCompanyProductPercent { get; init; }
    public IList<CreateProductAttributeOptionValueCommand> ProductAttributeOptionValues { get; init; } = new List<CreateProductAttributeOptionValueCommand>();
    public bool IsDefault { get; init; }
    public int ProductId { get; init; }
}
public class CreateProductAttributeOptionCommandHandler : IRequestHandler<CreateProductAttributeOptionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOptionValues = request.ProductAttributeOptionValues
                    .Select(productAttributeOptionValueCommand => new ProductAttributeOptionValue { Name = productAttributeOptionValueCommand.Name, Value = productAttributeOptionValueCommand.Value }).ToList();


        var entity = new ProductAttributeOption()
        {
            SafetyStockQty = request.SafetyStockQty,
            MinStockQty = request.MinStockQty,
            MaxStockQty = request.MaxStockQty,
            MaxSalePriceNonCompanyProductPercent = request.MaxSalePriceNonCompanyProductPercent,
            OptionValues = productAttributeOptionValues,
            IsDefault = request.IsDefault,
            ProductId = request.ProductId,
        };

        entity.SetTotalCount(request.TotalCount);

        var productAttributeOptions = await _context.ProductAttributeOptions.Where(c => c.ProductId == request.ProductId).ToListAsync(cancellationToken);
        if (request.IsDefault)
        {
            productAttributeOptions?.ForEach(d => d.IsDefault = false);
        }
        if (productAttributeOptions != null && productAttributeOptions.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        _context.ProductAttributeOptions.Add(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
