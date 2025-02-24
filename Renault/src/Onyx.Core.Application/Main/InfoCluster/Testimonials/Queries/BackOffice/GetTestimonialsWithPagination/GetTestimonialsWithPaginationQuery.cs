using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice.GetTestimonialsWithPagination;
public record GetTestimonialsWithPaginationQuery : IRequest<PaginatedList<TestimonialDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTestimonialsWithPaginationQueryHandler : IRequestHandler<GetTestimonialsWithPaginationQuery, PaginatedList<TestimonialDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTestimonialsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TestimonialDto>> Handle(GetTestimonialsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Testimonials
            .OrderBy(x => x.Name)
            .ProjectTo<TestimonialDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
