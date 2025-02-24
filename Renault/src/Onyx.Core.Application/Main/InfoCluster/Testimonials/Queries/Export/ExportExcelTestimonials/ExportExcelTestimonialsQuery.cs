using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.Export.ExportExcelTestimonials;

public record ExportExcelTestimonialsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelTestimonialsQueryHandler : IRequestHandler<ExportExcelTestimonialsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelTestimonialsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelTestimonialsQuery request, CancellationToken cancellationToken)
    {
        var testimonials = _context.Testimonials
            .OrderBy(x => x.Name);
        
        var selectedProperties = new List<Expression<Func<Testimonial, object>>?>()
        {
            c => c.Name,
            c => c.Position,
            c => c.Rating,
            c => c.Review,
            c => c.AboutUs,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            testimonials,
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
