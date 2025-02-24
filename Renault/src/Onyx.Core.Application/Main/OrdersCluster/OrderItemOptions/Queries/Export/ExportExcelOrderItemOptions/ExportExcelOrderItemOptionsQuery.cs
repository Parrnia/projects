using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.Export.ExportExcelOrderItemOptions;

public record ExportExcelOrderItemOptionsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelOrderItemOptionsQueryHandler : IRequestHandler<ExportExcelOrderItemOptionsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelOrderItemOptionsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelOrderItemOptionsQuery request, CancellationToken cancellationToken)
    {
        var orderItemOptions = _context.OrderItemOptions
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<OrderItemOption, object>>?>()
        {
            c => c.Name,
            c => c.Value,
            c => c.OrderItem,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            orderItemOptions,
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
