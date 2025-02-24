using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.Export.ExportExcelReturnOrderTotalDocuments;

public record ExportExcelReturnOrderTotalDocumentsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; } = 10;
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderTotalDocumentsQueryHandler : IRequestHandler<ExportExcelReturnOrderTotalDocumentsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderTotalDocumentsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderTotalDocumentsQuery request, CancellationToken cancellationToken)
    {
        var returnOrderTotalDocuments = _context.ReturnOrderTotalDocuments
            .OrderBy(x => x.Description);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderTotalDocument, object>>?>()
        {
            c => c.Image,
            c => c.Description,
            c => c.ReturnOrderTotalId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrderTotalDocuments,
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
