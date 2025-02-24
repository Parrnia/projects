using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;

public record GetReturnOrderInfoQuery(int Id) : IRequest<ReturnOrderInfoDto?>;

public class GetReturnOrderInfoQueryHandler : IRequestHandler<GetReturnOrderInfoQuery, ReturnOrderInfoDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnOrderInfoDto?> Handle(GetReturnOrderInfoQuery request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .ProjectTo<ReturnOrderInfoDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return returnOrder;
    }
}
