using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviews;

public record GetReviewsByProductIdQuery(int ProductId) : IRequest<List<ReviewDto>>;

public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, List<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReviewDto>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reviews.AsNoTracking()
            .Where(x => x.ProductId == request.ProductId)
            .OrderBy(x => x.ProductId)
            .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
