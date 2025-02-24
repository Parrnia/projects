using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariants;

public record GetAllVariantsByProductIdQuery(int ProductId) : IRequest<List<ProductDisplayVariantDto>>;

public class GetAllVariantsByProductIdQueryHandler : IRequestHandler<GetAllVariantsByProductIdQuery, List<ProductDisplayVariantDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVariantsByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDisplayVariantDto>> Handle(GetAllVariantsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(c => c.ProductDisplayVariants)
            .SingleOrDefaultAsync(c => c.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var variants = _mapper.Map<List<ProductDisplayVariant>, List<ProductDisplayVariantDto>>(product.ProductDisplayVariants);

        return variants;
    }
}