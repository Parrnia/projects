using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviews;

public record GetReviewsByCustomerIdQuery(Guid CustomerId) : IRequest<List<ReviewDto>>;

public class GetReviewsByCustomerIdQueryHandler : IRequestHandler<GetReviewsByCustomerIdQuery, List<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReviewDto>> Handle(GetReviewsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.ProductId)
            .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
