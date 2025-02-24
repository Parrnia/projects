using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.FrontOffice.GetProductAttributeOptions;
public record GetAllProductAttributeOptionsByProductIdQuery(int ProductId) : IRequest<List<AllProductAttributeOptionByProductIdDto>>;



public class GetAllProductAttributeOptionsByProductIdQueryHandler : IRequestHandler<GetAllProductAttributeOptionsByProductIdQuery, List<AllProductAttributeOptionByProductIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionsByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductAttributeOptionByProductIdDto>> Handle(GetAllProductAttributeOptionsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptions.AsNoTracking().Where(g => g.ProductId == request.ProductId)
            .OrderBy(x => x.Id)
            .ProjectTo<AllProductAttributeOptionByProductIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var productAttributeOption in res)
        {
            var list = productAttributeOption.Badges.Where(c => c.IsActive).ToList();
            productAttributeOption.Badges = list;
        }

        return res;
    }
}

