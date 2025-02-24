using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRolesWithPagination;
public record GetProductAttributeOptionRolesWithPaginationQuery : IRequest<PaginatedList<ProductAttributeOptionRoleWithPaginationDto>>
{
    public int ProductAttributeOptionId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductAttributeOptionRolesWithPaginationQueryHandler : IRequestHandler<GetProductAttributeOptionRolesWithPaginationQuery, PaginatedList<ProductAttributeOptionRoleWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRolesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductAttributeOptionRoleWithPaginationDto>> Handle(GetProductAttributeOptionRolesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
                .Where(x => x.ProductAttributeOptionId == request.ProductAttributeOptionId)
                .OrderBy(x => x.Availability)
                .ProjectTo<ProductAttributeOptionRoleWithPaginationDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

