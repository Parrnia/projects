using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public record GetAllConfirmedReturnOrdersWithPaginationQuery : IRequest<FilteredReturnOrderDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetAllConfirmedReturnOrdersWithPaginationQueryHandler : IRequestHandler<GetAllConfirmedReturnOrdersWithPaginationQuery, FilteredReturnOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllConfirmedReturnOrdersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredReturnOrderDto> Handle(GetAllConfirmedReturnOrdersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var returnReturnOrders = _context.ReturnOrders.OrderBy(c => c.CreatedAt).Where(c => c.ReturnOrderStateHistory.OrderBy(e => e.Created).Last().ReturnOrderStatus == ReturnOrderStatus.AllConfirmed);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            returnReturnOrders = returnReturnOrders.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            returnReturnOrders = returnReturnOrders.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await returnReturnOrders.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbReturnOrders = await returnReturnOrders.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<ReturnOrderDto>(_mapper.ConfigurationProvider);
        return new FilteredReturnOrderDto()
        {
            ReturnOrders = dbReturnOrders,
            Count = count
        };
    }
}
