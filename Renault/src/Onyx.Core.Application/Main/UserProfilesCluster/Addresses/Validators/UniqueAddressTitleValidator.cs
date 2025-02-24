using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Validators;

public record UniqueAddressTitleValidator : IRequest<bool>
{
    public Guid CustomerId { get; set; }
    public int AddressId { get; init; }
    public string Title { get; init; } = null!;
}

public class UniqueAddressTitleValidatorHandler : IRequestHandler<UniqueAddressTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueAddressTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueAddressTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Addresses.Where(c => c.CustomerId == request.CustomerId && c.Id != request.AddressId)
            .AllAsync(e => e.Title != request.Title, cancellationToken);
        return isUnique;
    }
}
    