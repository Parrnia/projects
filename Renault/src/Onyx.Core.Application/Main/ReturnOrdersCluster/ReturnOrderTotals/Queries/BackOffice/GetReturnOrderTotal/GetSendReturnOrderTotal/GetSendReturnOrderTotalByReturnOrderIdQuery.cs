using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotal.GetSendReturnOrderTotal;

public record GetSendReturnOrderTotalByReturnOrderIdQuery(int ReturnOrderId) : IRequest<SendReturnOrderTotalDto?>;


public class GetSendReturnOrderTotalByReturnOrderIdQueryHandler : IRequestHandler<GetSendReturnOrderTotalByReturnOrderIdQuery, SendReturnOrderTotalDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSendReturnOrderTotalByReturnOrderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SendReturnOrderTotalDto?> Handle(GetSendReturnOrderTotalByReturnOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrderTotals
            .ProjectTo<SendReturnOrderTotalDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => 
                x.ReturnOrderId == request.ReturnOrderId && 
                x.Type == ReturnOrderTotalType.ReturnShipping, 
                cancellationToken);
    }
}
