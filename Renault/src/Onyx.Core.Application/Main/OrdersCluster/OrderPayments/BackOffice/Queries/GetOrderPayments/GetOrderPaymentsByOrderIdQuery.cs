using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries.GetOrderPayments;

public record GetOrderPaymentsByOrderIdQuery(int OrderId) : IRequest<List<OrderPaymentDto>>;
public class GetOrderPaymentsByOrderIdQueryHandler : IRequestHandler<GetOrderPaymentsByOrderIdQuery, List<OrderPaymentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderPaymentsByOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderPaymentDto>> Handle(GetOrderPaymentsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.OrderPayments
            .Where(i => i.OrderId == request.OrderId)
            .OrderBy(x => x.OrderId)
            .ProjectTo<OrderPaymentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
