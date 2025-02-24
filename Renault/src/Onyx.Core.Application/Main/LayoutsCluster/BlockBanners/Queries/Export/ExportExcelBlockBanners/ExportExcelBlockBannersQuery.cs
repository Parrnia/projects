using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.Export.ExportExcelBlockBanners;

public record ExportExcelBlockBannersQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelBlockBannersQueryHandler : IRequestHandler<ExportExcelBlockBannersQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelBlockBannersQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelBlockBannersQuery request, CancellationToken cancellationToken)
    {
        var blockBanners = _context.BlockBanners
            .OrderBy(x => x.Title);
        
        var selectedProperties = new List<Expression<Func<BlockBanner, object>>?>()
        {
            c => c.Title,
            c => c.Subtitle,
            c => c.ButtonText,
            c => c.BlockBannerPosition,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            blockBanners,
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
