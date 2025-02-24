using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.FrontOffice.GetProductImages;

public record GetAllProductImagesQuery : IRequest<List<ProductImageDto>>;

public class GetAllProductImagesQueryHandler : IRequestHandler<GetAllProductImagesQuery, List<ProductImageDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductImagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductImageDto>> Handle(GetAllProductImagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductImages
            .OrderBy(x => x.Order)
            .ProjectTo<ProductImageDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
