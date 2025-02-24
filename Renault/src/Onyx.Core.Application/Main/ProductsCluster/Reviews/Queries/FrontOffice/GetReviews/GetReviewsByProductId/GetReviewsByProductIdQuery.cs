using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByProductId;

public record GetReviewsByProductIdQuery(int ProductId) : IRequest<List<ReviewByProductIdDto>>;

public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, List<ReviewByProductIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReviewByProductIdDto>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Where(x => x.ProductId == request.ProductId && x.IsActive)
            .OrderBy(x => x.ProductId)
            .ProjectTo<ReviewByProductIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
