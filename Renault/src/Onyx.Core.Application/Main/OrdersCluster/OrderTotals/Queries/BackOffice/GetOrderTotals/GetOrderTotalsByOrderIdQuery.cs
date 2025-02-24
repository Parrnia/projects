using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice.GetOrderTotals;

public record GetOrderTotalsByOrderIdQuery(int OrderId) : IRequest<List<OrderTotalDto>>;
public class GetOrderTotalsByOrderIdQueryHandler : IRequestHandler<GetOrderTotalsByOrderIdQuery, List<OrderTotalDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderTotalsByOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderTotalDto>> Handle(GetOrderTotalsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.OrderTotals.AsNoTracking()
            .Where(i => i.OrderId == request.OrderId)
            .OrderBy(x => x.OrderId)
            .ProjectTo<OrderTotalDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
