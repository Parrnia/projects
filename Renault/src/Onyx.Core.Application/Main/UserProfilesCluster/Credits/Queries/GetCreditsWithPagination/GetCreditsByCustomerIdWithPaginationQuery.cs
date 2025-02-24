using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditsWithPagination;
public record GetCreditsByCustomerIdWithPaginationQuery : IRequest<FilteredCreditDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; } = null!;
    public Guid CustomerId { get; init; }

}

public class GetCreditsByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetCreditsByCustomerIdWithPaginationQuery, FilteredCreditDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCreditsByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredCreditDto> Handle(GetCreditsByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var credits = _context.Credits
            .Where(c => c.CustomerId == request.CustomerId)
            .OrderBy(c => c.Date)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            credits = credits.ApplySearch(request.SearchTerm);
        }

        var count = await credits.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbCredits = await credits.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<CreditDto>(_mapper.ConfigurationProvider);
        return new FilteredCreditDto()
        {
            Credits = dbCredits,
            Count = count
        };
    }
}
