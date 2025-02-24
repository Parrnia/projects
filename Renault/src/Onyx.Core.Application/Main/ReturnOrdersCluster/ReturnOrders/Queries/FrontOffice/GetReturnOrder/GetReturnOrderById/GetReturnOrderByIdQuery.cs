using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderById;

public record GetReturnOrderByIdQuery(int Id, Guid? CustomerId) : IRequest<ReturnOrderByIdDto?>;

public class GetReturnOrderByIdQueryHandler : IRequestHandler<GetReturnOrderByIdQuery, ReturnOrderByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnOrderByIdDto?> Handle(GetReturnOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .ProjectTo<ReturnOrderByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (returnOrder == null)
        {
            return null;
        }
        if (request.CustomerId != null && returnOrder.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("GetReturnOrderByIdQuery");
        }

        return returnOrder;
    }
}
