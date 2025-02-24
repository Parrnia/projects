using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariants;
public record GetAllVariantsQuery : IRequest<List<ProductDisplayVariantDto>>;

public class GetAllVariantsQueryHandler : IRequestHandler<GetAllVariantsQuery, List<ProductDisplayVariantDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVariantsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDisplayVariantDto>> Handle(GetAllVariantsQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.ProductDisplayVariants
            .OrderBy(x => x.ProductId)
            .ProjectTo<ProductDisplayVariantDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return list;
    }
}
