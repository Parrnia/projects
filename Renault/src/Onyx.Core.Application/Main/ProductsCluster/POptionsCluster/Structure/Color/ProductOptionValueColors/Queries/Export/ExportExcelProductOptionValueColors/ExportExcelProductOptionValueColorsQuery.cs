using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.Export.ExportExcelProductOptionValueColors;

public record ExportExcelProductOptionValueColorsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductOptionValueColorsQueryHandler : IRequestHandler<ExportExcelProductOptionValueColorsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductOptionValueColorsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductOptionValueColorsQuery request, CancellationToken cancellationToken)
    {
        var productOptionValueColors = _context.ProductOptionValueColors
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductOptionValueColor, object>>?>()
        {
            c => c.Name,
            c => c.Slug,
            c => c.Color,
            c => c.ProductOptionColor,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productOptionValueColors,
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
