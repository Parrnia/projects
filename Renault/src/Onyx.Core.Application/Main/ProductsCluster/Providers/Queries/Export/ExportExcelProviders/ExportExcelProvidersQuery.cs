using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.Export.ExportExcelProviders;

public record ExportExcelProvidersQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelProvidersQueryHandler : IRequestHandler<ExportExcelProvidersQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelProvidersQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelProvidersQuery request, CancellationToken cancellationToken)
    {
        var providers = _context.Providers
            .OrderBy(x => x.LocalizedName);
        
        var selectedProperties = new List<Expression<Func<Provider, object>>?>()
        {
            c => c.Code,
            c => c.LocalizedName,
            c => c.Name,
            c => c.LocalizedCode,
            c => c.Description,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            providers,
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
