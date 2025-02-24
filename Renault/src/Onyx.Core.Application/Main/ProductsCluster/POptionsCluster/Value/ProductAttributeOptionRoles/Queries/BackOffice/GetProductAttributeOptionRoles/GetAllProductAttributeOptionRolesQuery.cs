using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRoles;
public record GetAllProductAttributeOptionRolesQuery : IRequest<List<ProductAttributeOptionRoleDto>>;

public class GetAllProductAttributeOptionRolesQueryHandler : IRequestHandler<GetAllProductAttributeOptionRolesQuery, List<ProductAttributeOptionRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeOptionRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionRoleDto>> Handle(GetAllProductAttributeOptionRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
            .OrderBy(x => x.Availability)
            .ProjectTo<ProductAttributeOptionRoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
