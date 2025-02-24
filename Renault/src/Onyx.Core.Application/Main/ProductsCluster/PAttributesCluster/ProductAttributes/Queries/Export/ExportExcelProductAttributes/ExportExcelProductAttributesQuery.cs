using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.Export.ExportExcelProductAttributes;

public record ExportExcelProductAttributesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductAttributesQueryHandler : IRequestHandler<ExportExcelProductAttributesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductAttributesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductAttributesQuery request, CancellationToken cancellationToken)
    {
        var productAttributes = _context.ProductAttributes
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductAttribute, object>>?>()
        {
            c => c.Name,
            c => c.Slug,
            c => c.Featured,
            c => c.ValueName,
            c => c.ValueSlug,
            c => c.Product,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productAttributes,
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
