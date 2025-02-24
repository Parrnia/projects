using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrders.GetReturnOrdersByOrderId;

public record GetReturnOrdersByOrderIdQuery(int OrderId, Guid CustomerId) : IRequest<List<ReturnOrderByOrderIdDto>>;

public class GetReturnOrdersByOrderIdWithPaginationQueryHandler : IRequestHandler<GetReturnOrdersByOrderIdQuery, List<ReturnOrderByOrderIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByOrderIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderByOrderIdDto>> Handle(GetReturnOrdersByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var returnOrders = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Where(x => x.Order.CustomerId == request.CustomerId && x.OrderId == request.OrderId)
            .OrderBy(x => x.Number)
            .ProjectTo<ReturnOrderByOrderIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return returnOrders;
    }
}
