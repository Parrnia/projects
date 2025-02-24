using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.Export.ExportExcelProductBrands;

public record ExportExcelProductBrandsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductBrandsQueryHandler : IRequestHandler<ExportExcelProductBrandsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductBrandsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = _context.ProductBrands
            .OrderBy(x => x.Name);

        var selectedProperties = new List<Expression<Func<ProductBrand, object>>?>()
        {
            c => c.LocalizedName,
            c => c.Name,
            c => c.Code,
            c => c.Slug,
            c => c.Country,
            c => c.IsActive,
        };

        var exported = await _exportServices.Export(
            brands,
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
