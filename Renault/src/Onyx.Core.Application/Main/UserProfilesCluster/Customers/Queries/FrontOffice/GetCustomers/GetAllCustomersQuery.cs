using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCustomers;
public record GetAllCustomersQuery : IRequest<List<BackOffice.CustomerDto>>;

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<BackOffice.CustomerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCustomersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BackOffice.CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Customers
            .OrderBy(x => x.Avatar)
            .ProjectTo<BackOffice.CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return users;
    }
}
