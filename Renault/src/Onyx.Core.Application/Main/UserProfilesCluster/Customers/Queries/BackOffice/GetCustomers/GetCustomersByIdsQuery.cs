 using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice.GetCustomers;

public record GetCustomersByIdsQuery : IRequest<List<CustomerDto>>
{
    public List<Guid> Ids { get; init; } = new List<Guid>();
}

public class GetCustomersByIdsQueryHandler : IRequestHandler<GetCustomersByIdsQuery, List<CustomerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersByIdsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersByIdsQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Customers
            .Where(c => request.Ids.Any(e => e == c.Id))
            .OrderBy(x => x.Avatar)
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var customerDto in users)
        {
            customerDto.Credits = customerDto.Credits.OrderByDescending(c => c.Date).ToList();
            customerDto.MaxCredits = customerDto.MaxCredits.OrderByDescending(c => c.Date).ToList();
        }
        return users;
    }
}
