using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice.GetReturnOrderItems;

public record GetReturnOrderItemsByReturnOrderItemGroupIdQuery(int ReturnOrderItemGroupId) : IRequest<List<ReturnOrderItemDto>>;


public class GetReturnOrderItemsByReturnOrderItemGroupIdQueryHandler : IRequestHandler<GetReturnOrderItemsByReturnOrderItemGroupIdQuery, List<ReturnOrderItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderItemsByReturnOrderItemGroupIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderItemDto>> Handle(GetReturnOrderItemsByReturnOrderItemGroupIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.ReturnOrderItems
            .Where(i => i.ReturnOrderItemGroupId == request.ReturnOrderItemGroupId)
            .OrderBy(x => x.Total)
            .ProjectTo<ReturnOrderItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
