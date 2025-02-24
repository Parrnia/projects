using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.Export.ExportExcelCountingUnits;

public record ExportExcelCountingUnitsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCountingUnitsQueryHandler : IRequestHandler<ExportExcelCountingUnitsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCountingUnitsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCountingUnitsQuery request, CancellationToken cancellationToken)
    {
        var countingUnits = _context.CountingUnits
            .OrderBy(x => x.LocalizedName);
        
        var selectedProperties = new List<Expression<Func<CountingUnit, object>>?>()
        {
            c => c.Code,
            c => c.LocalizedName,
            c => c.Name,
            c => c.IsDecimal,
            c => c.CountingUnitType,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            countingUnits,
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
