using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.Export.ExportExcelCustomerTypes;

public record ExportExcelCustomerTypesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCustomerTypesQueryHandler : IRequestHandler<ExportExcelCustomerTypesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCustomerTypesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCustomerTypesQuery request, CancellationToken cancellationToken)
    {
        var customerTypes = _context.CustomerTypes
            .OrderBy(x => x.CustomerTypeEnum);
        
        var selectedProperties = new List<Expression<Func<CustomerType, object>>?>()
        {
            c => c.CustomerTypeEnum,
            c => c.DiscountPercent,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            customerTypes,
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
