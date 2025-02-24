using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.Export.ExportExcelProductCategories;

public record ExportExcelProductCategoriesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProductCategoriesQueryHandler : IRequestHandler<ExportExcelProductCategoriesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProductCategoriesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var productCategories = _context.ProductCategories
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<ProductCategory, object>>?>()
        {
            c => c.Code,
            c => c.LocalizedName,
            c => c.Name,
            c => c.Slug,
            c => c.ProductCategoryNo,
            c => c.ProductParentCategory,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            productCategories,
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
