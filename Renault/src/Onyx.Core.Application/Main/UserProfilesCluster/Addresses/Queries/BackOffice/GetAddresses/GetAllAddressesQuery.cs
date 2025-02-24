using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddresses;
public record GetAllAddressesQuery : IRequest<List<AddressDto>>;

public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, List<AddressDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAddressesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Addresses
            .OrderBy(x => x.CustomerId)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
