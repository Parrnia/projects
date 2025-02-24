using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.Export.ExportExcelVariants;

public record ExportExcelVariantsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelVariantsQueryHandler : IRequestHandler<ExportExcelVariantsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelVariantsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelVariantsQuery request, CancellationToken cancellationToken)
    {
        var badges = _context.ProductDisplayVariants
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductDisplayVariant, object>>?>()
        {
            c => c.Name,
            c => c.IsBestSeller,
            c => c.IsFeatured,
            c => c.IsLatest,
            c => c.IsNew,
            c => c.IsPopular,
            c => c.IsSale,
            c => c.IsSpecialOffer,
            c => c.IsTopRated,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            badges,
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
