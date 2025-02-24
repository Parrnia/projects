using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrand.GetProductBrandById;

public record GetProductBrandByIdQuery(int Id) : IRequest<ProductBrandByIdDto?>;

public class GetProductBrandByIdQueryHandler : IRequestHandler<GetProductBrandByIdQuery, ProductBrandByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductBrandByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductBrandByIdDto?> Handle(GetProductBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductBrands
            .ProjectTo<ProductBrandByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
