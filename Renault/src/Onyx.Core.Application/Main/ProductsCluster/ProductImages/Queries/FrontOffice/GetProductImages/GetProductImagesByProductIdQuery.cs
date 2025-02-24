using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.FrontOffice.GetProductImages;

public record GetProductImagesByProductIdQuery(int ProductId) : IRequest<List<ProductImageDto>>;

public class GetProductImagesByProductIdQueryHandler : IRequestHandler<GetProductImagesByProductIdQuery, List<ProductImageDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductImagesByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductImageDto>> Handle(GetProductImagesByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductImages
            .Where(x => x.ProductId == request.ProductId)
            .OrderBy(x => x.Order)
            .ProjectTo<ProductImageDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
