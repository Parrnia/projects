using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.Export.ExportExcelCarousels;

public record ExportExcelCarouselsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCarouselsQueryHandler : IRequestHandler<ExportExcelCarouselsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCarouselsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCarouselsQuery request, CancellationToken cancellationToken)
    {
        var carousels = _context.Carousels
            .OrderBy(x => x.Title);
        
        var selectedProperties = new List<Expression<Func<Carousel, object>>?>()
        {
            c => c.Url,
            c => c.Offer,
            c => c.Title,
            c => c.Details,
            c => c.ButtonLabel,
            c => c.Order,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            carousels,
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
