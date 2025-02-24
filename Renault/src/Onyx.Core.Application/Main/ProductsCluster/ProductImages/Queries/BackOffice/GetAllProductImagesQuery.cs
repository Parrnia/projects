using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.BackOffice;

public record GetAllProductImagesQuery : IRequest<List<ProductImagesDto>>;

public class GetAllProductImagesQueryHandler : IRequestHandler<GetAllProductImagesQuery, List<ProductImagesDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductImagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductImagesDto>> Handle(GetAllProductImagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductImages.AsNoTracking()
            .OrderBy(x => x.Order)
            .ProjectTo<ProductImagesDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
