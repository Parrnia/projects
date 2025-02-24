using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByCustomerId;

public record GetReviewsByCustomerIdQuery(Guid CustomerId) : IRequest<List<ReviewByCustomerIdDto>>;

public class GetReviewsByCustomerIdQueryHandler : IRequestHandler<GetReviewsByCustomerIdQuery, List<ReviewByCustomerIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReviewByCustomerIdDto>> Handle(GetReviewsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Where(x => x.CustomerId == request.CustomerId && x.IsActive)
            .OrderBy(x => x.ProductId)
            .ProjectTo<ReviewByCustomerIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
