using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.Export.ExportExcelProductOptionMaterials;

public record ExportExcelProductOptionMaterialsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductOptionMaterialsQueryHandler : IRequestHandler<ExportExcelProductOptionMaterialsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductOptionMaterialsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductOptionMaterialsQuery request, CancellationToken cancellationToken)
    {
        var productOptionMaterials = _context.ProductOptionMaterials
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductOptionMaterial, object>>?>()
        {
            c => c.Name,
            c => c.Slug,
            c => c.Type,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productOptionMaterials,
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
