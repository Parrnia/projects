using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelCompletedOrders;

public record ExportExcelCompletedOrdersQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCompletedOrdersQueryHandler : IRequestHandler<ExportExcelCompletedOrdersQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCompletedOrdersQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCompletedOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = _context.Orders
            .Where(c => c.OrderStateHistory.OrderBy(e => e.Created).Last().OrderStatus == OrderStatus.OrderCompleted)
            .OrderBy(x => x.CustomerLastName);
        
        var selectedProperties = new List<Expression<Func<Order, object>>?>()
        {
            c => c.Token,
            c => c.Number,
            c => c.Quantity,
            c => c.Subtotal,
            c => c.DiscountPercentForRole,
            c => c.Total,
            c => c.CreatedAt,
            c => c.OrderPaymentType,
            c => c.OrderStateHistory,
            c => c.Items,
            c => c.Totals,
            c => c.OrderAddress,
            c => c.CustomerTypeEnum,
            c => c.PhoneNumber,
            c => c.CustomerFirstName,
            c => c.CustomerLastName,
            c => c.NationalCode,
            c => c.PersonType,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            orders,
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
