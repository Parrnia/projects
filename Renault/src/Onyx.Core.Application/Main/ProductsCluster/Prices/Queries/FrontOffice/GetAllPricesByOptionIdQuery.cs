using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.FrontOffice;

public record GetAllPricesByOptionIdQuery(int OptionId) : IRequest<List<AllPriceByOptionIdDto>>;

public class GetAllPricesByOptionIdQueryHandler : IRequestHandler<GetAllPricesByOptionIdQuery, List<AllPriceByOptionIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPricesByOptionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllPriceByOptionIdDto>> Handle(GetAllPricesByOptionIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Prices.AsNoTracking().Where(p=>p.ProductAttributeOptionId==request.OptionId)
            .OrderBy(x => x.Date)
            .ProjectTo<AllPriceByOptionIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}