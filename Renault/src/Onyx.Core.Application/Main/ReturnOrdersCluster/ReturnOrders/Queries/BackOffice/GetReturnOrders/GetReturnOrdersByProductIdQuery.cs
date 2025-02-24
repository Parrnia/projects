using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;
public record GetReturnOrdersByProductIdQuery(int ProductId) : IRequest<List<ReturnOrderDto>>;

public class GetReturnOrdersByProductIdQueryHandler : IRequestHandler<GetReturnOrdersByProductIdQuery, List<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderDto>> Handle(GetReturnOrdersByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders.AsNoTracking()
            .Where(x => x.Order.Items.Any(i => i.ProductAttributeOption.Product.Id == request.ProductId))
            .OrderBy(x => x.Number)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
