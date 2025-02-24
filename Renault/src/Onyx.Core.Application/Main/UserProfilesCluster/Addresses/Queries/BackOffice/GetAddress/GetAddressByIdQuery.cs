using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddress;

public record GetAddressByIdQuery(int Id) : IRequest<AddressDto?>;

public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAddressByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return address;
    }
}
