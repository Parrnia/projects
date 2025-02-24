using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.Export.ExportExcelFooterLinkContainers;

public record ExportExcelFooterLinkContainersQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelFooterLinkContainersQueryHandler : IRequestHandler<ExportExcelFooterLinkContainersQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelFooterLinkContainersQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelFooterLinkContainersQuery request, CancellationToken cancellationToken)
    {
        var footerLinkContainers = _context.FooterLinkContainers
            .OrderBy(x => x.Header);
        
        var selectedProperties = new List<Expression<Func<FooterLinkContainer, object>>?>()
        {
            c => c.Header,
            c => c.Order,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            footerLinkContainers,
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
