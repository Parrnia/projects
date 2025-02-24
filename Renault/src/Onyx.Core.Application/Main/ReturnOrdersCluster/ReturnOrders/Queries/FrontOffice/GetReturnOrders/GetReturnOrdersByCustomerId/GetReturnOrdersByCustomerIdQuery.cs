using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrders.GetReturnOrdersByCustomerId;

public record GetReturnOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<List<ReturnOrderByCustomerIdDto>>;

public class GetReturnOrdersByCustomerIdQueryHandler : IRequestHandler<GetReturnOrdersByCustomerIdQuery, List<ReturnOrderByCustomerIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderByCustomerIdDto>> Handle(GetReturnOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var returnOrders = await _context.ReturnOrders
            .Where(x => x.Order.CustomerId == request.CustomerId)
            .OrderBy(x => x.Number)
            .ProjectTo<ReturnOrderByCustomerIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return returnOrders;
    }
}
