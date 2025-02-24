using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.Export.ExportExcelProductStatuses;

public record ExportExcelProductStatusesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductStatusesQueryHandler : IRequestHandler<ExportExcelProductStatusesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductStatusesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductStatusesQuery request, CancellationToken cancellationToken)
    {
        var productStatusEnumerable = _context.ProductStatuses
            .OrderBy(x => x.LocalizedName);
        
        var selectedProperties = new List<Expression<Func<ProductStatus, object>>?>()
        {
            c => c.Name,
            c => c.LocalizedName,
            c => c.Code,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productStatusEnumerable,
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
