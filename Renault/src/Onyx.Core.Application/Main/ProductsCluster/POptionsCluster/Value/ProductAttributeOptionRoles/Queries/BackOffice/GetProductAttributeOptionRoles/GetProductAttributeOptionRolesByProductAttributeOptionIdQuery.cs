using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRoles;

public record GetProductAttributeOptionRolesByProductAttributeOptionIdQuery(int ProductAttributeOptionId) : IRequest<List<ProductAttributeOptionRoleDto>>;

public class GetProductAttributeOptionRolesByProductAttributeOptionIdQueryHandler : IRequestHandler<GetProductAttributeOptionRolesByProductAttributeOptionIdQuery, List<ProductAttributeOptionRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRolesByProductAttributeOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionRoleDto>> Handle(GetProductAttributeOptionRolesByProductAttributeOptionIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductAttributeOptionRoles
            .Where(x => x.ProductAttributeOptionId == request.ProductAttributeOptionId)
            .OrderBy(x => x.Availability)
            .ProjectTo<ProductAttributeOptionRoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}
