using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRolesWithPagination;
public record GetProductAttributeOptionRolesWithPaginationQuery : IRequest<PaginatedList<ProductAttributeOptionRoleDto>>
{
    public int ProductAttributeOptionId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductAttributeOptionRolesWithPaginationQueryHandler : IRequestHandler<GetProductAttributeOptionRolesWithPaginationQuery, PaginatedList<ProductAttributeOptionRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRolesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductAttributeOptionRoleDto>> Handle(GetProductAttributeOptionRolesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
                .Where(x => x.ProductAttributeOptionId == request.ProductAttributeOptionId)
                .OrderBy(x => x.Availability)
                .ProjectTo<ProductAttributeOptionRoleDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

