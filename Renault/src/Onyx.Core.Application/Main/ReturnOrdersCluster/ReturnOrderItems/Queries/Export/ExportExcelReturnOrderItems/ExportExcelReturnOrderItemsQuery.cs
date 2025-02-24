using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.Export.ExportExcelReturnOrderItems;

public record ExportExcelReturnOrderItemsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderItemsQueryHandler : IRequestHandler<ExportExcelReturnOrderItemsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderItemsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var returnOrderItems = _context.ReturnOrderItems
            .OrderBy(x => x.Total);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderItem, object>>?>()
        {
            c => c.Quantity,
            c => c.Total,
            c => c.IsAccepted,
            c => c.ReturnOrderItemGroup,
            c => c.ReturnOrderReason,
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
