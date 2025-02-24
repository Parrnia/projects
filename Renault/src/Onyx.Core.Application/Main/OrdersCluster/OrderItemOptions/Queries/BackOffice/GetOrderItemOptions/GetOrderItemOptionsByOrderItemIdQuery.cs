using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.BackOffice.GetOrderItemOptions;

public record GetOrderItemOptionsByOrderItemIdQuery(int OrderItemId) : IRequest<List<OrderItemOptionDto>>;

public class GetOrderItemOptionsByOrderItemIdQueryHandler : IRequestHandler<GetOrderItemOptionsByOrderItemIdQuery, List<OrderItemOptionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderItemOptionsByOrderItemIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderItemOptionDto>> Handle(GetOrderItemOptionsByOrderItemIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.OrderItemOptions.AsNoTracking()
            .Where(i => i.OrderItemId == request.OrderItemId)
            .OrderBy(x => x.OrderItemId)
            .ProjectTo<OrderItemOptionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
