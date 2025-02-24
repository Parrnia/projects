using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.Export.ExportExcelCustomerTickets;

public record ExportExcelSocialLinksQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelSocialLinksQueryHandler : IRequestHandler<ExportExcelSocialLinksQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelSocialLinksQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelSocialLinksQuery request, CancellationToken cancellationToken)
    {
        var socialLinks = _context.SocialLinks
            .OrderBy(x => x.Url);
        
        var selectedProperties = new List<Expression<Func<SocialLink, object>>?>()
        {
            c => c.Url,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            socialLinks,
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
