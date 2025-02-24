using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.Export.ExportExcelCustomerTickets;

public record ExportExcelCustomerTicketsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelCustomerTicketsQueryHandler : IRequestHandler<ExportExcelCustomerTicketsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelCustomerTicketsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelCustomerTicketsQuery request, CancellationToken cancellationToken)
    {
        var customerTickets = _context.CustomerTickets
            .OrderBy(x => x.CustomerName);
        
        var selectedProperties = new List<Expression<Func<CustomerTicket, object>>?>()
        {
            c => c.Subject,
            c => c.Message,
            c => c.Date,
            c => c.CustomerPhoneNumber,
            c => c.CustomerName,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            customerTickets,
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
