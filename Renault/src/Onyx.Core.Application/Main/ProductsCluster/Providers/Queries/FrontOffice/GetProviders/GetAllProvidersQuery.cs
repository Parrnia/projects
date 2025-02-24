using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProviders;
public record GetAllProvidersQuery : IRequest<List<AllProviderDto>>;

public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, List<AllProviderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProvidersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProviderDto>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProviderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
