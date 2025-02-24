using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttribute;

public record GetProductAttributeByIdQuery(int Id) : IRequest<ProductAttributeDto?>;

public class GetProductAttributeByIdQueryHandler : IRequestHandler<GetProductAttributeByIdQuery, ProductAttributeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttributeDto?> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributes
            .ProjectTo<ProductAttributeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}

