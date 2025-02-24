using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Queries.BackOffice;

public record GetAllProductAttributeOptionValueByOptionIdQuery(int Id) : IRequest<List<ProductAttributeOptionValueDto>>;

public class GetAllProductAttributeOptionValueByOptionIdQueryHandler : IRequestHandler<GetAllProductAttributeOptionValueByOptionIdQuery, List<ProductAttributeOptionValueDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionValueByOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionValueDto>> Handle(GetAllProductAttributeOptionValueByOptionIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptionValues.AsNoTracking().Where(g => g.ProductAttributeOptionId == request.Id)
            .OrderBy(x => x.Id)
            .ProjectTo<ProductAttributeOptionValueDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}

