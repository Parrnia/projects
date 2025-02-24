using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarousel;

public record GetCarouselByIdQuery(int Id) : IRequest<CarouselDto?>;

public class GetCarouselByIdQueryHandler : IRequestHandler<GetCarouselByIdQuery, CarouselDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCarouselByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CarouselDto?> Handle(GetCarouselByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Carousels
            .ProjectTo<CarouselDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
