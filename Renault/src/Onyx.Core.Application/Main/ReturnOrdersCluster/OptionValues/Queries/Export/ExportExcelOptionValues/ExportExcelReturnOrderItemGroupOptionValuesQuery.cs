using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.Export.ExportExcelOptionValues;

public record ExportExcelReturnOrderItemGroupOptionValuesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderItemGroupOptionValuesQueryHandler : IRequestHandler<ExportExcelReturnOrderItemGroupOptionValuesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderItemGroupOptionValuesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderItemGroupOptionValuesQuery request, CancellationToken cancellationToken)
    {
        var returnOrderItemGroupOptionValues = _context.ReturnOrderItemGroupProductAttributeOptionValues
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderItemGroupProductAttributeOptionValue, object>>?>()
        {
            c => c.Name,
            c => c.Value,
            c => c.ReturnOrderItemGroup,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrderItemGroupOptionValues,
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
