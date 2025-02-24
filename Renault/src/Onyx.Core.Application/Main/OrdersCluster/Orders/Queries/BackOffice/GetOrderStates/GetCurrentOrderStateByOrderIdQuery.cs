using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;

public record GetCurrentOrderStateByOrderIdQuery(int OrderId) : IRequest<OrderStateBaseDto?>;


public class GetCurrentOrderStateByOrderIdQueryHandler : IRequestHandler<GetCurrentOrderStateByOrderIdQuery, OrderStateBaseDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrentOrderStateByOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderStateBaseDto?> Handle(GetCurrentOrderStateByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.OrderStateHistory)
            .ProjectTo<OrderDtoForState>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(i => i.Id == request.OrderId, cancellationToken);
        var currentState = _mapper.Map<OrderStateBaseDto?>(order?.CurrentOrderStateBase);
        return currentState;
    }
}
