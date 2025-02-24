using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotals;

public record GetReturnOrderTotalsByReturnOrderIdQuery(int ReturnOrderId) : IRequest<List<ReturnOrderTotalDto>>;


public class GetReturnOrderTotalsByReturnOrderIdQueryHandler : IRequestHandler<GetReturnOrderTotalsByReturnOrderIdQuery, List<ReturnOrderTotalDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderTotalsByReturnOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderTotalDto>> Handle(GetReturnOrderTotalsByReturnOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrderTotals
            .Where(i => i.ReturnOrderId == request.ReturnOrderId)
            .OrderBy(x => x.Title)
            .ProjectTo<ReturnOrderTotalDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
