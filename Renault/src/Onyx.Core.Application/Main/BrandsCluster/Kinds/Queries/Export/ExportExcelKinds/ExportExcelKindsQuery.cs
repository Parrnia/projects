using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.Export.ExportExcelKinds;

public record ExportExcelKindsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelKindsQueryHandler : IRequestHandler<ExportExcelKindsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelKindsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelKindsQuery request, CancellationToken cancellationToken)
    {
        var kinds = _context.Kinds
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<Kind, object>>?>()
        {
            c => c.LocalizedName,
            c => c.Name,
            c => c.Model,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            kinds,
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
