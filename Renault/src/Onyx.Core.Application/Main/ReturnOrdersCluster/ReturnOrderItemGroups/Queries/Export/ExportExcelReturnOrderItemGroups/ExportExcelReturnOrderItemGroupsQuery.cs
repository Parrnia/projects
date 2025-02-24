using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.Export.ExportExcelReturnOrderItemGroups;

public record ExportExcelReturnOrderItemGroupsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderItemGroupsQueryHandler : IRequestHandler<ExportExcelReturnOrderItemGroupsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderItemGroupsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderItemGroupsQuery request, CancellationToken cancellationToken)
    {
        var returnOrderItems = _context.ReturnOrderItemGroups
            .OrderBy(x => x.ProductLocalizedName);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderItemGroup, object>>?>()
        {
            c => c.Price,
            c => c.TotalDiscountPercent,
            c => c.ProductLocalizedName,
            c => c.ProductName,
            c => c.ProductNo,
            c => c.ProductAttributeOption,
            c => c.ReturnOrder,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrderItems,
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
