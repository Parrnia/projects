using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColor;

public record GetProductOptionColorByIdQuery(int Id) : IRequest<ProductOptionColorByIdDto?>;

public class GetProductOptionColorByIdQueryHandler : IRequestHandler<GetProductOptionColorByIdQuery, ProductOptionColorByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductOptionColorByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductOptionColorByIdDto?> Handle(GetProductOptionColorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionColors
            .ProjectTo<ProductOptionColorByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
