using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelRejectedReturnOrders;

public record ExportExcelRejectedReturnOrdersQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelRejectedReturnOrdersQueryHandler : IRequestHandler<ExportExcelRejectedReturnOrdersQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelRejectedReturnOrdersQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelRejectedReturnOrdersQuery request, CancellationToken cancellationToken)
    {
        var returnOrders = _context.ReturnOrders
            .Where(c => c.ReturnOrderStateHistory.OrderBy(e => e.Created).Last().ReturnOrderStatus == ReturnOrderStatus.Rejected)
            .OrderBy(x => x.Number);
        
        var selectedProperties = new List<Expression<Func<ReturnOrder, object>>?>()
        {
            c => c.Token,
            c => c.Number,
            c => c.Quantity,
            c => c.Subtotal,
            c => c.Total,
            c => c.CreatedAt,
            c => c.CostRefundType,
            c => c.ReturnOrderStateHistory,
            c => c.ReturnOrderTransportationType,
            c => c.Totals,
            c => c.Order,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrders,
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
