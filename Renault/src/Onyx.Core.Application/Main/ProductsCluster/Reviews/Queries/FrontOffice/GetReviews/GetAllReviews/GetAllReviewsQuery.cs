using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetAllReviews;
public record GetAllReviewsQuery : IRequest<List<AllReviewDto>>;

public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, List<AllReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReviewsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllReviewDto>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Reviews
            .Where(x => x.IsActive)
            .OrderBy(x => x.CustomerId)
            .ProjectTo<AllReviewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
