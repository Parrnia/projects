using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductTypes;
public record GetAllProductTypesDropDownQuery : IRequest<List<AllProductTypeDropDownDto>>;

public class GetAllProductTypesDropDownQueryHandler : IRequestHandler<GetAllProductTypesDropDownQuery, List<AllProductTypeDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypesDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductTypeDropDownDto>> Handle(GetAllProductTypesDropDownQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductTypeDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
