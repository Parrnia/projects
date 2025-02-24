using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrderStates;

public record GetReturnOrderStatesByReturnOrderIdQuery(int ReturnOrderId) : IRequest<List<ReturnOrderStateBaseDto>>;


public class GetReturnOrderStatesByReturnOrderIdQueryHandler : IRequestHandler<GetReturnOrderStatesByReturnOrderIdQuery, List<ReturnOrderStateBaseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderStatesByReturnOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderStateBaseDto>> Handle(GetReturnOrderStatesByReturnOrderIdQuery request, CancellationToken cancellationToken)
    {
        var states = await _context.ReturnOrderStateBases
            .Where(i => i.ReturnOrderId == request.ReturnOrderId)
            .OrderBy(x => x.ReturnOrderStatus)
            .ProjectTo<ReturnOrderStateBaseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return states;
    }
}
