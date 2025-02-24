using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRole;

public record GetProductAttributeOptionRoleByIdQuery(int Id) : IRequest<ProductAttributeOptionRoleDto?>;

public class GetProductAttributeOptionRoleByIdQueryHandler : IRequestHandler<GetProductAttributeOptionRoleByIdQuery, ProductAttributeOptionRoleDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttributeOptionRoleDto?> Handle(GetProductAttributeOptionRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
            .ProjectTo<ProductAttributeOptionRoleDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
