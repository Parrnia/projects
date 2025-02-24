using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.Export.ExportExcelProductAttributeOptionRoles;

public record ExportExcelProductAttributeOptionRolesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductAttributeOptionRolesQueryHandler : IRequestHandler<ExportExcelProductAttributeOptionRolesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductAttributeOptionRolesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductAttributeOptionRolesQuery request, CancellationToken cancellationToken)
    {
        var productAttributeOptionRoles = _context.ProductAttributeOptionRoles
            .OrderBy(x => x.CustomerTypeEnum);
        
        var selectedProperties = new List<Expression<Func<ProductAttributeOptionRole, object>>?>()
        {
            c => c.MinimumStockToDisplayProductForThisCustomerTypeEnum,
            c => c.Availability,
            c => c.MainMaxOrderQty,
            c => c.CurrentMinOrderQty,
            c => c.MainMinOrderQty,
            c => c.CurrentMinOrderQty,
            c => c.CustomerTypeEnum,
            c => c.DiscountPercent,
            c => c.ProductAttributeOptionId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productAttributeOptionRoles,
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
