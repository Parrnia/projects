using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.Export.ExportExcelAddresses;

public record ExportExcelAddressesQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelAddressesQueryHandler : IRequestHandler<ExportExcelAddressesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelAddressesQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = _context.Addresses
            .OrderBy(x => x.Title);
        
        var selectedProperties = new List<Expression<Func<Address, object>>?>()
        {
            c => c.Title,
            c => c.Company,
            c => c.Country,
            c => c.AddressDetails1,
            c => c.AddressDetails2,
            c => c.City,
            c => c.State,
            c => c.Postcode,
            c => c.Default,
            c => c.CustomerId,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            addresses,
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
