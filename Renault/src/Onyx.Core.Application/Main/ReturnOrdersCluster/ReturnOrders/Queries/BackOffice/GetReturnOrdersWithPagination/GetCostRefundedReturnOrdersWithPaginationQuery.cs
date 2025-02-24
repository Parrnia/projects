using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public record GetCostRefundedReturnOrdersWithPaginationQuery : IRequest<FilteredReturnOrderDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetCostRefundedReturnOrdersWithPaginationQueryHandler : IRequestHandler<GetCostRefundedReturnOrdersWithPaginationQuery, FilteredReturnOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCostRefundedReturnOrdersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredReturnOrderDto> Handle(GetCostRefundedReturnOrdersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var returnOrders = _context.ReturnOrders.OrderBy(c => c.CreatedAt).Where(c => c.ReturnOrderStateHistory.OrderBy(e => e.Created).Last().ReturnOrderStatus == ReturnOrderStatus.CostRefunded);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            returnOrders = returnOrders.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            returnOrders = returnOrders.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await returnOrders.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbReturnOrders = await returnOrders.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<ReturnOrderDto>(_mapper.ConfigurationProvider);
        return new FilteredReturnOrderDto()
        {
            ReturnOrders = dbReturnOrders,
            Count = count
        };
    }
}
