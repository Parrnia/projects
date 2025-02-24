using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrderStates;

public record GetCurrentReturnOrderStateByReturnOrderIdQuery(int ReturnOrderId) : IRequest<ReturnOrderStateBaseDto?>;


public class GetCurrentReturnOrderStateByReturnOrderIdQueryHandler : IRequestHandler<GetCurrentReturnOrderStateByReturnOrderIdQuery, ReturnOrderStateBaseDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrentReturnOrderStateByReturnOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnOrderStateBaseDto?> Handle(GetCurrentReturnOrderStateByReturnOrderIdQuery request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .ProjectTo<ReturnOrderDtoForState>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(i => i.Id == request.ReturnOrderId, cancellationToken);
        var currentState = _mapper.Map<ReturnOrderStateBaseDto?>(returnOrder?.CurrentReturnOrderStateBase);
        return currentState;
    }
}
