using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice;

namespace Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditByCustomerId;

public record GetMaxCreditByCustomerIdQuery : IRequest<decimal?>
{
    public Guid CustomerId { get; set; }
}

public class GetMaxCreditByCustomerIdQueryHandler : IRequestHandler<GetMaxCreditByCustomerIdQuery, decimal?>
{
    private readonly IApplicationDbContext _context;

    public GetMaxCreditByCustomerIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> Handle(GetMaxCreditByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var credit = await _context.MaxCredits
            .OrderBy(c => c.Created)
            .LastOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        
        return credit?.Value;
    }
}
