using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;

public record GetReturnOrderByIdQuery(int Id) : IRequest<ReturnOrderDto?>;

public class GetReturnOrderByIdQueryHandler : IRequestHandler<GetReturnOrderByIdQuery, ReturnOrderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnOrderDto?> Handle(GetReturnOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.ReturnOrders
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return order;
    }
}
