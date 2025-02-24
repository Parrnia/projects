using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.BackOffice;

public record GetAllProductOptionValueColorsByColorIdQuery(int colorId) : IRequest<List<ProductOptionValueColorDto>>;

public class GetAllProductOptionValueColorsByColorIdQueryHandler : IRequestHandler<GetAllProductOptionValueColorsByColorIdQuery, List<ProductOptionValueColorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueColorsByColorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionValueColorDto>> Handle(GetAllProductOptionValueColorsByColorIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionValueColors.AsNoTracking()
            .Where(x => x.ProductOptionColorId == request.colorId)
            .OrderBy(x => x.ProductOptionColorId)
            .ProjectTo<ProductOptionValueColorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}