using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProviders;
public record GetAllProvidersQuery : IRequest<List<ProviderDto>>;

public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, List<ProviderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProvidersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProviderDto>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProviderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
