using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;

public record GetReturnOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<List<ReturnOrderDto>>;

public class GetReturnOrdersByCustomerIdQueryHandler : IRequestHandler<GetReturnOrdersByCustomerIdQuery, List<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderDto>> Handle(GetReturnOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders
            .Where(x => x.Order.CustomerId == request.CustomerId)
            .OrderBy(x => x.Number)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
