using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;
public record GetAllProductAttributeOptionsByProductIdQuery(int ProductId) : IRequest<List<ProductAttributeOptionDto>>;



public class GetAllProductAttributeOptionByProductIdQueryHandler : IRequestHandler<GetAllProductAttributeOptionsByProductIdQuery, List<ProductAttributeOptionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionDto>> Handle(GetAllProductAttributeOptionsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptions.AsNoTracking().Where(g => g.ProductId == request.ProductId)
            .OrderBy(x => x.Id)
            .ProjectTo<ProductAttributeOptionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}

