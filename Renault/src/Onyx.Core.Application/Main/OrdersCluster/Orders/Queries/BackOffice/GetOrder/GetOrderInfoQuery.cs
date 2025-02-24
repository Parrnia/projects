using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrder;

public record GetOrderInfoQuery(int Id) : IRequest<OrderInfoDto?>;

public class GetOrderInfoQueryHandler : IRequestHandler<GetOrderInfoQuery, OrderInfoDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderInfoDto?> Handle(GetOrderInfoQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .ProjectTo<OrderInfoDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return order;
    }
}
