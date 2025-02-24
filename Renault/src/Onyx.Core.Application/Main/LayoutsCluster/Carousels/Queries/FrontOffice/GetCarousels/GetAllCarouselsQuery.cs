using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.FrontOffice.GetCarousels;
public record GetAllCarouselsQuery : IRequest<List<CarouselDto>>;

public class GetAllCarouselsQueryHandler : IRequestHandler<GetAllCarouselsQuery, List<CarouselDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCarouselsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CarouselDto>> Handle(GetAllCarouselsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Carousels
            .Where(c => c.IsActive)
            .OrderBy(x => x.Order)
            .ProjectTo<CarouselDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
