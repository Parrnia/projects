using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductTypes;
public record GetAllProductTypesQuery : IRequest<List<ProductTypeDto>>;

public class GetAllProductTypesQueryHandler : IRequestHandler<GetAllProductTypesQuery, List<ProductTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductTypeDto>> Handle(GetAllProductTypesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
