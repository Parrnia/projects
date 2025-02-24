using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddress.GetDefaultAddress;

public record GetDefaultAddressQuery(Guid CustomerId) : IRequest<AddressDto?>;

public class GetDefaultAddressQueryHandler : IRequestHandler<GetDefaultAddressQuery, AddressDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDefaultAddressQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AddressDto?> Handle(GetDefaultAddressQuery request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.City)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Default == true, cancellationToken: cancellationToken);
        return address;
    }
}
