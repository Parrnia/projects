using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Queries.FrontOffice;

public record GetAllProductAttributeOptionValuesByOptionIdQuery(int OptionId) : IRequest<List<AllProductAttributeOptionValueByOptionIdDto>>;

public class GetAllProductAttributeOptionValueByOptionIdQueryHandler : IRequestHandler<GetAllProductAttributeOptionValuesByOptionIdQuery, List<AllProductAttributeOptionValueByOptionIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionValueByOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductAttributeOptionValueByOptionIdDto>> Handle(GetAllProductAttributeOptionValuesByOptionIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptionValues.AsNoTracking().Where(g => g.ProductAttributeOptionId == request.OptionId)
            .OrderBy(x => x.Id)
            .ProjectTo<AllProductAttributeOptionValueByOptionIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}

