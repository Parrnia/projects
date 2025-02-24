using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.Export.ExportExcelProductTypeAttributeGroupAttributes;

public record ExportExcelProductTypeAttributeGroupAttributesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductTypeAttributeGroupAttributesQueryHandler : IRequestHandler<ExportExcelProductTypeAttributeGroupAttributesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductTypeAttributeGroupAttributesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductTypeAttributeGroupAttributesQuery request, CancellationToken cancellationToken)
    {
        var productTypeAttributeGroupAttributes = _context.ProductTypeAttributeGroupAttributes
            .OrderBy(x => x.Value);
        
        var selectedProperties = new List<Expression<Func<ProductTypeAttributeGroupAttribute, object>>?>()
        {
            c => c.Value,
            c => c.ProductTypeAttributeGroup,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productTypeAttributeGroupAttributes,
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
