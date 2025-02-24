using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrand;

public record GetProductBrandByIdQuery(int Id) : IRequest<ProductBrandDto?>;

public class GetProductBrandByIdQueryHandler : IRequestHandler<GetProductBrandByIdQuery, ProductBrandDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductBrandByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductBrandDto?> Handle(GetProductBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductBrands
            .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
