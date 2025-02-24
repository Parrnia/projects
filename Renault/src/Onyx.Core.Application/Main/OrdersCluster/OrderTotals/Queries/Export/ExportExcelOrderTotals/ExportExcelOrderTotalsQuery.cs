using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.Export.ExportExcelOrderTotals;

public record ExportExcelOrderTotalsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelOrderTotalsQueryHandler : IRequestHandler<ExportExcelOrderTotalsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelOrderTotalsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelOrderTotalsQuery request, CancellationToken cancellationToken)
    {
        var orderTotals = _context.OrderTotals
            .OrderBy(x => x.Title);

        var selectedProperties = new List<Expression<Func<OrderTotal, object>>?>()
        {
            c => c.Title,
            c => c.Price,
            c => c.Type,
            c => c.OrderId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            orderTotals,
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
