using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountries;
public record GetAllCountriesDropDownQuery : IRequest<List<AllCountryDropDownDto>>;

public class GetAllCountriesDropDownQueryHandler : IRequestHandler<GetAllCountriesDropDownQuery, List<AllCountryDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountriesDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllCountryDropDownDto>> Handle(GetAllCountriesDropDownQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllCountryDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
