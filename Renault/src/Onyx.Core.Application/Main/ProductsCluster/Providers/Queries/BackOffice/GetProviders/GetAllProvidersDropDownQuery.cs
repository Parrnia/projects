using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProviders;
public record GetAllProvidersDropDownQuery : IRequest<List<AllProviderDropDownDto>>;

public class GetAllProvidersDropDownQueryHandler : IRequestHandler<GetAllProvidersDropDownQuery, List<AllProviderDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProvidersDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProviderDropDownDto>> Handle(GetAllProvidersDropDownQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProviderDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
