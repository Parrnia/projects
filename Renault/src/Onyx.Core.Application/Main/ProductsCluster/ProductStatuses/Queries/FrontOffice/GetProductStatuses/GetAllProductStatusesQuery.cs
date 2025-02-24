using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.FrontOffice.GetProductStatuses;
public record GetAllProductStatusesQuery : IRequest<List<AllProductStatusDto>>;

public class GetAllProductStatusesQueryHandler : IRequestHandler<GetAllProductStatusesQuery, List<AllProductStatusDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductStatusesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductStatusDto>> Handle(GetAllProductStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductStatusDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
