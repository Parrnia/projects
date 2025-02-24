using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;

namespace Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditsWithPagination;
public record GetMaxCreditsByCustomerIdWithPaginationQuery : IRequest<FilteredMaxCreditDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; } = null!;
    public Guid CustomerId { get; init; }

}

public class GetMaxCreditsByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetMaxCreditsByCustomerIdWithPaginationQuery, FilteredMaxCreditDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMaxCreditsByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredMaxCreditDto> Handle(GetMaxCreditsByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var maxCredits = _context.MaxCredits
            .Where(c => c.CustomerId == request.CustomerId)
            .OrderBy(c => c.Date)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            maxCredits = maxCredits.ApplySearch(request.SearchTerm);
        }

        var count = await maxCredits.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbMaxCredits = await maxCredits.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<MaxCreditDto>(_mapper.ConfigurationProvider);
        return new FilteredMaxCreditDto()
        {
            MaxCredits = dbMaxCredits,
            Count = count
        };
    }
}
