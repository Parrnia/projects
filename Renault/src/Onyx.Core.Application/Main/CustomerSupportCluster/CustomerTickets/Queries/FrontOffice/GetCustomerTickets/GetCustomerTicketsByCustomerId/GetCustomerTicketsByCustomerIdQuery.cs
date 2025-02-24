using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.FrontOffice.GetCustomerTickets.GetCustomerTicketsByCustomerId;

public record GetCustomerTicketsByCustomerIdQuery(Guid CustomerId) : IRequest<List<CustomerTicketByCustomerIdDto>>;

public class GetCustomerTicketsByCustomerIdQueryHandler : IRequestHandler<GetCustomerTicketsByCustomerIdQuery, List<CustomerTicketByCustomerIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTicketsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CustomerTicketByCustomerIdDto>> Handle(GetCustomerTicketsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CustomerTickets
            .Where(x => x.CustomerId == request.CustomerId && x.IsActive == true)
            .OrderBy(x => x.Subject)
            .ProjectTo<CustomerTicketByCustomerIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
