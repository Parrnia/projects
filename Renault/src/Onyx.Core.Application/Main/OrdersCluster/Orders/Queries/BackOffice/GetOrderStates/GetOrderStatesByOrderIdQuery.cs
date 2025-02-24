using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;

public record GetOrderStatesByOrderIdQuery(int OrderId) : IRequest<List<OrderStateBaseDto>>;


public class GetOrderStatesByOrderIdQueryHandler : IRequestHandler<GetOrderStatesByOrderIdQuery, List<OrderStateBaseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderStatesByOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderStateBaseDto>> Handle(GetOrderStatesByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var states = await _context.OrderStateBases
            .Where(i => i.OrderId == request.OrderId)
            .OrderBy(x => x.Created)
            .ProjectTo<OrderStateBaseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return states;
    }
}
