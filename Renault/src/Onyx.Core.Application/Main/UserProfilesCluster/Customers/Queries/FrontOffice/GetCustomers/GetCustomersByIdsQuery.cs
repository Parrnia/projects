using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCustomers;

public record GetCustomersByIdsQuery : IRequest<List<BackOffice.CustomerDto>>
{
    public List<Guid> Ids { get; init; } = new List<Guid>();
}

public class GetCustomersByIdsQueryHandler : IRequestHandler<GetCustomersByIdsQuery, List<BackOffice.CustomerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersByIdsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BackOffice.CustomerDto>> Handle(GetCustomersByIdsQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Customers
            .Where(c => request.Ids.Any(e => e == c.Id))
            .OrderBy(x => x.Avatar)
            .ProjectTo<BackOffice.CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return users;
    }
}
