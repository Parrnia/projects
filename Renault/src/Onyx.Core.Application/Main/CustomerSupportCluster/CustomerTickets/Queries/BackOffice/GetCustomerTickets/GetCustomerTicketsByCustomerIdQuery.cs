using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTickets;

public record GetCustomerTicketsByCustomerIdQuery(Guid CustomerId) : IRequest<List<CustomerTicketDto>>;

public class GetCustomerTicketsByCustomerIdQueryHandler : IRequestHandler<GetCustomerTicketsByCustomerIdQuery, List<CustomerTicketDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTicketsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CustomerTicketDto>> Handle(GetCustomerTicketsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CustomerTickets
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.Subject)
            .ProjectTo<CustomerTicketDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
