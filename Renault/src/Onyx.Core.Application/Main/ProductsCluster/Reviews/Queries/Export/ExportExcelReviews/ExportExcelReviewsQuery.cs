using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.Export.ExportExcelReviews;

public record ExportExcelReviewsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelReviewsQueryHandler : IRequestHandler<ExportExcelReviewsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelReviewsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = _context.Reviews
            .OrderBy(x => x.ProductId);

        var selectedProperties = new List<Expression<Func<Review, object>>?>()
        {
            c => c.Date,
            c => c.Rating,
            c => c.Content,
            c => c.AuthorName,
            c => c.Product,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            reviews,
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
