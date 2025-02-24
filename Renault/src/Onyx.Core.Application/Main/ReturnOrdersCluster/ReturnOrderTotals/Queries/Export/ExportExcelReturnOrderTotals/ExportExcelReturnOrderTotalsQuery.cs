using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.Export.ExportExcelReturnOrderTotals;

public record ExportExcelReturnOrderTotalsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderTotalsQueryHandler : IRequestHandler<ExportExcelReturnOrderTotalsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderTotalsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderTotalsQuery request, CancellationToken cancellationToken)
    {
        var returnOrderTotals = _context.ReturnOrderTotals
            .OrderBy(x => x.Title);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderTotal, object>>?>()
        {
            c => c.Title,
            c => c.Price,
            c => c.Type,
            c => c.ReturnOrder,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrderTotals,
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
