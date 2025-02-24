using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountries;
public record GetAllCountriesQuery : IRequest<List<AllCountryDto>>;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, List<AllCountryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllCountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllCountryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
