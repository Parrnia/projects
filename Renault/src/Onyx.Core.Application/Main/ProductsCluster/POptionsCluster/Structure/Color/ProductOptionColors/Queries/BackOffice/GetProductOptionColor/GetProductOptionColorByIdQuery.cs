using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice.GetProductOptionColor;

public record GetProductOptionColorByIdQuery(int Id) : IRequest<ProductOptionColorDto?>;

public class GetProductOptionColorByIdQueryHandler : IRequestHandler<GetProductOptionColorByIdQuery, ProductOptionColorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductOptionColorByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductOptionColorDto?> Handle(GetProductOptionColorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionColors
            .ProjectTo<ProductOptionColorDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
