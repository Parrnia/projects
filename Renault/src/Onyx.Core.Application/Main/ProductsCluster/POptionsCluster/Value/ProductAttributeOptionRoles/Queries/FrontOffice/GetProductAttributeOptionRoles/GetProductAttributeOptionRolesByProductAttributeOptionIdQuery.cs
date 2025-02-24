using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRoles;

public record GetProductAttributeOptionRolesByProductAttributeOptionIdQuery(int ProductAttributeOptionId) : IRequest<List<ProductAttributeOptionRoleByProductAttributeOptionIdDto>>;

public class GetProductAttributeOptionRolesByProductAttributeOptionIdQueryHandler : IRequestHandler<GetProductAttributeOptionRolesByProductAttributeOptionIdQuery, List<ProductAttributeOptionRoleByProductAttributeOptionIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRolesByProductAttributeOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeOptionRoleByProductAttributeOptionIdDto>> Handle(GetProductAttributeOptionRolesByProductAttributeOptionIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
            .Where(x => x.ProductAttributeOptionId == request.ProductAttributeOptionId)
            .OrderBy(x => x.Availability)
            .ProjectTo<ProductAttributeOptionRoleByProductAttributeOptionIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
