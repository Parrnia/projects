using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderByNumber;

public record GetReturnOrderByNumberQuery() : IRequest<ReturnOrderByNumberDto?>
{
    public string Number { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
}

public class GetReturnOrderByNumberQueryHandler : IRequestHandler<GetReturnOrderByNumberQuery, ReturnOrderByNumberDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderByNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnOrderByNumberDto?> Handle(GetReturnOrderByNumberQuery request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .ProjectTo<ReturnOrderByNumberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Number == request.Number && x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (returnOrder == null)
        {
            throw new NotFoundException("سفارش مورد نظر یافت نشد");
        }
        return returnOrder;
    }
}
