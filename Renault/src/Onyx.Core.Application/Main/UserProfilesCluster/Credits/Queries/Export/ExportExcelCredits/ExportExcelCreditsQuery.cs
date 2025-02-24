using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.Export.ExportExcelCredits;

public record ExportExcelCreditsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCreditsQueryHandler : IRequestHandler<ExportExcelCreditsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCreditsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCreditsQuery request, CancellationToken cancellationToken)
    {
        var credits = _context.Credits
            .OrderBy(x => x.Value);
        
        var selectedProperties = new List<Expression<Func<Credit, object>>?>()
        {
            c => c.Date,
            c => c.Value,
            c => c.Date,
            c => c.ModifierUserName,
            c => c.OrderToken,
            c => c.CustomerId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            credits,
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
