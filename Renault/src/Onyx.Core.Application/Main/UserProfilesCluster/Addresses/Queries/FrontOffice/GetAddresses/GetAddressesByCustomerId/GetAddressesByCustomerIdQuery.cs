using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddresses.GetAddressesByCustomerId;

public record GetAddressesByCustomerIdQuery(Guid CustomerId) : IRequest<List<AddressDto>>;

public class GetAddressesByCustomerIdQueryHandler : IRequestHandler<GetAddressesByCustomerIdQuery, List<AddressDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAddressesByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AddressDto>> Handle(GetAddressesByCustomerIdQuery request, CancellationToken cancellationToken)
    {

        var result = await _context.Addresses
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.City)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
