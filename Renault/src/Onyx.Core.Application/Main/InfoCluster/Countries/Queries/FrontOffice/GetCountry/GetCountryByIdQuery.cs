using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountry;

public record GetCountryByIdQuery(int Id) : IRequest<CountryByIdDto?>;

public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountryByIdDto?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .ProjectTo<CountryByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
