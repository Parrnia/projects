using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatus;

public record GetProductStatusByIdQuery(int Id) : IRequest<ProductStatusDto?>;

public class GetProductStatusByIdQueryHandler : IRequestHandler<GetProductStatusByIdQuery, ProductStatusDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStatusByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductStatusDto?> Handle(GetProductStatusByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses
            .ProjectTo<ProductStatusDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
