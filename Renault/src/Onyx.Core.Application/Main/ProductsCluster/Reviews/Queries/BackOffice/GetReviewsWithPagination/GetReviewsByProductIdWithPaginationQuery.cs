using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviewsWithPagination;
public record GetReviewsByProductIdWithPaginationQuery : IRequest<PaginatedList<ReviewDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int ProductId { get; init; }
}

public class GetReviewsByProductIdWithPaginationQueryHandler : IRequestHandler<GetReviewsByProductIdWithPaginationQuery, PaginatedList<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByProductIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReviewDto>> Handle(GetReviewsByProductIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .OrderBy(x => x.CustomerId)
            .Where(c => c.ProductId == request.ProductId)
            .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
