using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.Export.ExportExcelProducts;

public record ExportExcelProductsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductsQueryHandler : IRequestHandler<ExportExcelProductsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .OrderBy(x => x.LocalizedName);
        
        var selectedProperties = new List<Expression<Func<Product, object>>?>()
        {
            c => c.Code,
            c => c.ProductNo,
            c => c.OldProductNo,
            c => c.LocalizedName,
            c => c.Name,
            c => c.ProductCatalog,
            c => c.OrderRate,
            c => c.Height,
            c => c.Width,
            c => c.Length,
            c => c.NetWeight,
            c => c.GrossWeight,
            c => c.VolumeWeight,
            c => c.Mileage,
            c => c.Duration,
            c => c.Provider,
            c => c.Country,
            c => c.ProductType,
            c => c.ProductStatus,
            c => c.MainCountingUnit,
            c => c.CommonCountingUnit,
            c => c.ProductBrand,
            c => c.ProductCategory,
            c => c.Excerpt,
            c => c.Description,
            c => c.Slug,
            c => c.Sku,
            c => c.Compatibility,
            c => c.ProductAttributeType,
            c => c.Tags,
            c => c.ColorOption,
            c => c.MaterialOption,
            c => c.Excerpt,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            products,
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
