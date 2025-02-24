using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRole;

public record GetProductAttributeOptionRoleByIdQuery(int Id) : IRequest<ProductAttributeOptionRoleByIdDto?>;

public class GetProductAttributeOptionRoleByIdQueryHandler : IRequestHandler<GetProductAttributeOptionRoleByIdQuery, ProductAttributeOptionRoleByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeOptionRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttributeOptionRoleByIdDto?> Handle(GetProductAttributeOptionRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeOptionRoles
            .ProjectTo<ProductAttributeOptionRoleByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
