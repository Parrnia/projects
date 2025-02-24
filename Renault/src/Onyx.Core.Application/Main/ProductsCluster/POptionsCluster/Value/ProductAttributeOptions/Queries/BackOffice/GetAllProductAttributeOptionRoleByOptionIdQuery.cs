using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;

public record GetAllProductAttributeOptionRoleByOptionIdQuery(int Id) : IRequest<List<ProductAttributeOptionRoleDto>>;

public class GetAllProductAttributeOptionRoleByOptionIdQueryHandler : IRequestHandler<GetAllProductAttributeOptionRoleByOptionIdQuery, List<ProductAttributeOptionRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionRoleByOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionRoleDto>> Handle(GetAllProductAttributeOptionRoleByOptionIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptionRoles
            .Where(g => g.ProductAttributeOptionId == request.Id)
            .OrderBy(x => x.Id)
            .ProjectTo<ProductAttributeOptionRoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}

