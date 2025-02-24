using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.Export.ExportExcelVehicles;

public record ExportExcelVehiclesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelVehiclesQueryHandler : IRequestHandler<ExportExcelVehiclesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelVehiclesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = _context.Vehicles
            .OrderBy(x => x.Customer);
        
        var selectedProperties = new List<Expression<Func<Vehicle, object>>?>()
        {
            c => c.VinNumber,
            c => c.Kind,
            c => c.CustomerId
        };

        var exported = await _exportServices.Export(
            vehicles,
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
