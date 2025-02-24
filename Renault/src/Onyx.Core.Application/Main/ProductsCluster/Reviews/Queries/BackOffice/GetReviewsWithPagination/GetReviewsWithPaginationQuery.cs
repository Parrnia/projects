using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviewsWithPagination;
public record GetReviewsWithPaginationQuery : IRequest<FilteredReviewDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetReviewsWithPaginationQueryHandler : IRequestHandler<GetReviewsWithPaginationQuery, FilteredReviewDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredReviewDto> Handle(GetReviewsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var reviews = _context.Reviews.OrderBy(c => c.Date).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            reviews = reviews.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            reviews = reviews.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await reviews.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbReviews = await reviews.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<ReviewDto>(_mapper.ConfigurationProvider);
        return new FilteredReviewDto()
        {
            Reviews = dbReviews,
            Count = count
        };
    }
}
