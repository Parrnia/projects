using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Validators;

public record UniqueAddressPostcodeValidator : IRequest<bool>
{
    public Guid CustomerId { get; set; }
    public int AddressId { get; init; }
    public string Postcode { get; init; } = null!;
}

public class UniqueAddressPostcodeValidatorHandler : IRequestHandler<UniqueAddressPostcodeValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueAddressPostcodeValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueAddressPostcodeValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Addresses.Where(c => c.CustomerId == request.CustomerId && c.Id != request.AddressId)
            .AllAsync(e => e.Postcode != request.Postcode, cancellationToken);
        return isUnique;
    }
}
    