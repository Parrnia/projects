using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.FrontOffice.GetCustomerTicket;

public record GetCustomerTicketByIdQuery(int Id) : IRequest<CustomerTicketByIdDto?>;

public class GetCustomerTicketByIdQueryHandler : IRequestHandler<GetCustomerTicketByIdQuery, CustomerTicketByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTicketByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerTicketByIdDto?> Handle(GetCustomerTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _context.CustomerTickets
            .ProjectTo<CustomerTicketByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return reviews;
    }
}
