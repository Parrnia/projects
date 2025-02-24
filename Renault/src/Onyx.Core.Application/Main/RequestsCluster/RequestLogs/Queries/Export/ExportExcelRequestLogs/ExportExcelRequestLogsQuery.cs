using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.RequestsCluster;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.Export.ExportExcelRequestLogs;

public record ExportExcelRequestLogsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelRequestLogsQueryHandler : IRequestHandler<ExportExcelRequestLogsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelRequestLogsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelRequestLogsQuery request, CancellationToken cancellationToken)
    {
        var requestLogs = _context.RequestLogs
            .OrderBy(x => x.ApiType);
        
        var selectedProperties = new List<Expression<Func<RequestLog, object>>?>()
        {
            c => c.ApiAddress,
            c => c.RequestBody,
            c => c.ErrorMessage,
            c => c.ResponseStatus,
            c => c.Created,
            c => c.RequestType,
            c => c.ApiType
        };

        var exported = await _exportServices.Export(
            requestLogs,
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
