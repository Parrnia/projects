using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductKinds.Queries.BackOffice;


public record GetAllProductKindsByProductIdQuery(int ProductId) : IRequest<List<AllProductKindByProductIdDto>>;

public class GetAllProductKindsByProductIdQueryHandler : IRequestHandler<GetAllProductKindsByProductIdQuery, List<AllProductKindByProductIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductKindsByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductKindByProductIdDto>> Handle(GetAllProductKindsByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductKinds
            .Where(x => x.ProductId == request.ProductId)
            .OrderBy(x => x.KindId)
            .ProjectTo<AllProductKindByProductIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}

