using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.Export.ExportExcelReturnOrderItemDocuments;

public record ExportExcelReturnOrderItemDocumentsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReturnOrderItemDocumentsQueryHandler : IRequestHandler<ExportExcelReturnOrderItemDocumentsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReturnOrderItemDocumentsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReturnOrderItemDocumentsQuery request, CancellationToken cancellationToken)
    {
        var returnOrderItemDocuments = _context.ReturnOrderItemDocuments
            .OrderBy(x => x.Description);
        
        var selectedProperties = new List<Expression<Func<ReturnOrderItemDocument, object>>?>()
        {
            c => c.Image,
            c => c.Description,
            c => c.ReturnOrderItemId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            returnOrderItemDocuments,
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
