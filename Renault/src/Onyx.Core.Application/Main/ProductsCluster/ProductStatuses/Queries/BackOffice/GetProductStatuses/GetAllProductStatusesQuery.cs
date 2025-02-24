using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatuses;
public record GetAllProductStatusesQuery : IRequest<List<ProductStatusDto>>;

public class GetAllProductStatusesQueryHandler : IRequestHandler<GetAllProductStatusesQuery, List<ProductStatusDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductStatusesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductStatusDto>> Handle(GetAllProductStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductStatusDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
