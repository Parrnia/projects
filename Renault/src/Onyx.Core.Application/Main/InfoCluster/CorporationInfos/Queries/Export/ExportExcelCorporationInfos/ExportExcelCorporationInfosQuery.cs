using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.Export.ExportExcelCorporationInfos;

public record ExportExcelCorporationInfosQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCorporationInfosQueryHandler : IRequestHandler<ExportExcelCorporationInfosQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCorporationInfosQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCorporationInfosQuery request, CancellationToken cancellationToken)
    {
        var corporationInfos = _context.CorporationInfos
            .OrderBy(x => x.PoweredBy);
        
        var selectedProperties = new List<Expression<Func<CorporationInfo, object>>?>()
        {
            c => c.ContactUsMessage,
            c => c.PoweredBy,
            c => c.CallUs,
            c => c.IsDefault,
            c => c.PhoneNumbers,
            c => c.EmailAddresses,
            c => c.LocationAddresses,
            c => c.WorkingHours,
        };

        var exported = await _exportServices.Export(
            corporationInfos,
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
