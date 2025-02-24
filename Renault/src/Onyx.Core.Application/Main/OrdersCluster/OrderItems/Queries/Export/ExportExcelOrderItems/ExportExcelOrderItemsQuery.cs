using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Queries.Export.ExportExcelOrderItems;

public record ExportExcelOrderItemsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelOrderItemsQueryHandler : IRequestHandler<ExportExcelOrderItemsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelOrderItemsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var orderItems = _context.OrderItems
            .OrderBy(x => x.ProductLocalizedName);
        
        var selectedProperties = new List<Expression<Func<OrderItem, object>>?>()
        {
            c => c.Price,
            c => c.DiscountPercentForProduct,
            c => c.Quantity,
            c => c.Total,
            c => c.OrderId,
            c => c.ProductLocalizedName,
            c => c.ProductName,
            c => c.ProductNo,
            c => c.ProductAttributeOptionId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            orderItems,
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
