using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.Export.ExportExcelProductTypeAttributeGroups;

public record ExportExcelProductTypeAttributeGroupsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductTypeAttributeGroupsQueryHandler : IRequestHandler<ExportExcelProductTypeAttributeGroupsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductTypeAttributeGroupsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductTypeAttributeGroupsQuery request, CancellationToken cancellationToken)
    {
        var productTypeAttributeGroups = _context.ProductTypeAttributeGroups
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductTypeAttributeGroup, object>>?>()
        {
            c => c.Name,
            c => c.Slug,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productTypeAttributeGroups,
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
