using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.BackOffice;

public record GetAllPricesByOptionIdQuery(int OptionId) : IRequest<List<PriceDto>>;

public class GetAllPricesByProductIdQueryHandler : IRequestHandler<GetAllPricesByOptionIdQuery, List<PriceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPricesByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PriceDto>> Handle(GetAllPricesByOptionIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Prices.AsNoTracking().Where(p => p.ProductAttributeOptionId == request.OptionId)
            .OrderBy(x => x.Date)
            .ProjectTo<PriceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}