using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice.GetOrderItems;

public record GetOrderItemsByOrderIdQuery(int OrderId) : IRequest<List<OrderItemDto>>;


public class GetOrderItemsByOrderIdQueryHandler : IRequestHandler<GetOrderItemsByOrderIdQuery, List<OrderItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderItemsByOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderItemDto>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.OrderItems.AsNoTracking()
            .Where(i => i.OrderId == request.OrderId)
            .OrderBy(x => x.OrderId)
            .ProjectTo<OrderItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
