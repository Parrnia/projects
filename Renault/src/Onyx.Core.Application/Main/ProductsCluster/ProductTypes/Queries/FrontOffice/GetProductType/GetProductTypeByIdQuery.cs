using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductType;

public record GetProductTypeByIdQuery(int Id) : IRequest<ProductTypeByIdDto?>;

public class GetProductTypeByIdQueryHandler : IRequestHandler<GetProductTypeByIdQuery, ProductTypeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductTypeByIdDto?> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes
            .ProjectTo<ProductTypeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
