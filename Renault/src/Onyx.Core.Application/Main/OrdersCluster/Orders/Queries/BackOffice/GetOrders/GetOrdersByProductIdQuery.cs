using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrders;
public record GetOrdersByProductIdQuery(int ProductId) : IRequest<List<OrderDto>>;

public class GetOrdersByProductIdQueryHandler : IRequestHandler<GetOrdersByProductIdQuery, List<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders.AsNoTracking()
            .Where(x => x.Items.Any(i => i.ProductAttributeOption.Product.Id == request.ProductId))
            .OrderBy(x => x.Number)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
