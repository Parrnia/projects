using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.Export.ExportExcelFooterLinks;

public record ExportExcelFooterLinksQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelFooterLinksQueryHandler : IRequestHandler<ExportExcelFooterLinksQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelFooterLinksQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelFooterLinksQuery request, CancellationToken cancellationToken)
    {
        var footerLinks = _context.FooterLinks
            .OrderBy(x => x.Title);
        
        var selectedProperties = new List<Expression<Func<FooterLink, object>>?>()
        {
            c => c.Title,
            c => c.Url,
            c => c.IsActive,
            c => c.FooterLinkContainer,
        };

        var exported = await _exportServices.Export(
            footerLinks,
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
