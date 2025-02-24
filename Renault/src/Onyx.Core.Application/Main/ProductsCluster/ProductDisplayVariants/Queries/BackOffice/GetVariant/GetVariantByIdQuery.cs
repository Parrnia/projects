using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariant;

public record GetVariantByIdQuery(int Id) : IRequest<ProductDisplayVariantDto?>;

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, ProductDisplayVariantDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVariantByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDisplayVariantDto?> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductDisplayVariants
            .ProjectTo<ProductDisplayVariantDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}
