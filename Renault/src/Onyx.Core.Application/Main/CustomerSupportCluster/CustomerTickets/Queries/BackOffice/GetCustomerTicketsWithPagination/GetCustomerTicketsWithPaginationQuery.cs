using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicketsWithPagination;
public record GetCustomerTicketsWithPaginationQuery : IRequest<FilteredCustomerTicketDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetCustomerTicketsWithPaginationQueryHandler : IRequestHandler<GetCustomerTicketsWithPaginationQuery, FilteredCustomerTicketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTicketsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredCustomerTicketDto> Handle(GetCustomerTicketsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var reviews = _context.CustomerTickets.OrderBy(c => c.Subject).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            reviews = reviews.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            reviews = reviews.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await reviews.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbCustomerTickets = await reviews.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<CustomerTicketDto>(_mapper.ConfigurationProvider);
        return new FilteredCustomerTicketDto()
        {
            CustomerTickets = dbCustomerTickets,
            Count = count
        };
    }
}
