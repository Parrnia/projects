using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditByCustomerId;

public record GetCreditByCustomerIdQuery : IRequest<decimal?>
{
    public Guid CustomerId { get; set; }
}

public class GetCreditByCustomerIdQueryHandler : IRequestHandler<GetCreditByCustomerIdQuery, decimal?>
{
    private readonly IApplicationDbContext _context;

    public GetCreditByCustomerIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> Handle(GetCreditByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var credit = await _context.Credits
            .OrderBy(c => c.Created)
            .LastOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        
        return credit?.Value;
    }
}
