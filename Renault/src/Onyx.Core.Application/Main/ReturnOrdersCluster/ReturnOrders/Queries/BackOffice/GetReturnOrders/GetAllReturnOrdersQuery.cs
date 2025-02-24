using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;
public record GetAllReturnOrdersQuery : IRequest<List<ReturnOrderDto>>;

public class GetAllReturnOrdersQueryHandler : IRequestHandler<GetAllReturnOrdersQuery, List<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReturnOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderDto>> Handle(GetAllReturnOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders
            .OrderBy(x => x.Number)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
