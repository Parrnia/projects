using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProvider;

public record GetProviderByIdQuery(int Id) : IRequest<ProviderDto?>;

public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, ProviderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProviderDto?> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers
            .ProjectTo<ProviderDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
