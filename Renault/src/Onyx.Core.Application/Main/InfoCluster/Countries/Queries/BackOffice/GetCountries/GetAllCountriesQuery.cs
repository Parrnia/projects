using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountries;
public record GetAllCountriesQuery : IRequest<List<CountryDto>>;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, List<CountryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
