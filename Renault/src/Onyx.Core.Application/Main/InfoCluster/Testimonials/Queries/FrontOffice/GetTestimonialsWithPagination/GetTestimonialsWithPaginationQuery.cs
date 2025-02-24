using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonialsWithPagination;
public record GetTestimonialsWithPaginationQuery : IRequest<PaginatedList<TestimonialWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTestimonialsWithPaginationQueryHandler : IRequestHandler<GetTestimonialsWithPaginationQuery, PaginatedList<TestimonialWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTestimonialsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TestimonialWithPaginationDto>> Handle(GetTestimonialsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Testimonials
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<TestimonialWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
