using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCredits;
public record GetCreditsByCustomerIdWithPaginationQuery : IRequest<PaginatedList<CreditDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid CustomerId { get; set; }
}

public class GetCreditsByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetCreditsByCustomerIdWithPaginationQuery, PaginatedList<CreditDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCreditsByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CreditDto>> Handle(GetCreditsByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var credits = await _context.Credits
            .Where(c => c.CustomerId == request.CustomerId)
            .OrderBy(x => x.Date)
            .ProjectTo<CreditDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);


        return credits;
    }
}
