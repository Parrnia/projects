using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice.GetReturnOrderItemGroups;

public record GetReturnOrderItemGroupsByReturnOrderIdQuery(int ReturnOrderId) : IRequest<List<ReturnOrderItemGroupDto>>;


public class GetReturnOrderItemGroupsByReturnOrderIdQueryHandler : IRequestHandler<GetReturnOrderItemGroupsByReturnOrderIdQuery, List<ReturnOrderItemGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderItemGroupsByReturnOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderItemGroupDto>> Handle(GetReturnOrderItemGroupsByReturnOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrderItemGroups
            .Include(c => c.ReturnOrderItems)
            .Where(i => i.ReturnOrderId == request.ReturnOrderId)
            .OrderBy(x => x.ProductLocalizedName)
            .ProjectTo<ReturnOrderItemGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
