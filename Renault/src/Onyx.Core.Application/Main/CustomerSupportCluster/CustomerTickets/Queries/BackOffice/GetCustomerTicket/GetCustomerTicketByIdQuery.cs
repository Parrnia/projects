using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicket;

public record GetCustomerTicketByIdQuery(int Id) : IRequest<CustomerTicketDto?>;

public class GetCustomerTicketByIdQueryHandler : IRequestHandler<GetCustomerTicketByIdQuery, CustomerTicketDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTicketByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerTicketDto?> Handle(GetCustomerTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var customerTicket = await _context.CustomerTickets
            .ProjectTo<CustomerTicketDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return customerTicket;
    }
}
