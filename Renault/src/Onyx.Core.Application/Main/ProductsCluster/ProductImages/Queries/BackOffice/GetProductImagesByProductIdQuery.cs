using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.BackOffice;

public record GetProductImagesByProductIdQuery(int ProductId) : IRequest<List<ProductImagesDto>>;

public class GetProductImagesByProductIdQueryHandler : IRequestHandler<GetProductImagesByProductIdQuery, List<ProductImagesDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductImagesByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductImagesDto>> Handle(GetProductImagesByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductImages.AsNoTracking()
            .Where(x => x.ProductId == request.ProductId)
            .OrderBy(x => x.Order)
            .ProjectTo<ProductImagesDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
