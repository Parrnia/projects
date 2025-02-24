using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatuses;
public record GetAllProductStatusesDropDownQuery : IRequest<List<AllProductStatusDropDownDto>>;

public class GetAllProductStatusesDropDownQueryHandler : IRequestHandler<GetAllProductStatusesDropDownQuery, List<AllProductStatusDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductStatusesDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductStatusDropDownDto>> Handle(GetAllProductStatusesDropDownQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductStatusDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
