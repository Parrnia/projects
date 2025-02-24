using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice.GetProductAttributeType;

public record GetProductAttributeTypeByIdQuery(int Id) : IRequest<ProductAttributeTypeByIdDto?>;

public class GetProductAttributeTypeByIdQueryHandler : IRequestHandler<GetProductAttributeTypeByIdQuery, ProductAttributeTypeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttributeTypeByIdDto?> Handle(GetProductAttributeTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeTypes
            .ProjectTo<ProductAttributeTypeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
