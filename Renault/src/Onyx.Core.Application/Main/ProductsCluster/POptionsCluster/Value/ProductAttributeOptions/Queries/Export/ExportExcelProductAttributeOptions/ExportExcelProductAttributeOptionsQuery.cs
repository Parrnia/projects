using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.Export.ExportExcelProductAttributeOptions;

public record ExportExcelProductAttributeOptionsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductAttributeOptionsQueryHandler : IRequestHandler<ExportExcelProductAttributeOptionsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductAttributeOptionsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductAttributeOptionsQuery request, CancellationToken cancellationToken)
    {
        var productAttributeOptions = _context.ProductAttributeOptions
            .OrderBy(x => x.ProductId);
        
        var selectedProperties = new List<Expression<Func<ProductAttributeOption, object>>?>()
        {
            c => c.TotalCount,
            c => c.SafetyStockQty,
            c => c.MinStockQty,
            c => c.MaxStockQty,
            c => c.Prices,
            c => c.MaxSalePriceNonCompanyProductPercent,
            c => c.Badges,
            c => c.OptionValues,
            c => c.IsDefault,
            c => c.Product,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productAttributeOptions,
            selectedProperties,
            request.SearchText,
            request.PageNumber,
            request.PageSize,
            request.StartCreationDate,
            request.EndCreationDate,
            request.StartChangeDate,
            request.EndChangeDate,
            cancellationToken);

        var exportedExcel = _exportServices.ExportExcel(exported, selectedProperties);
        return exportedExcel;
    }
}
