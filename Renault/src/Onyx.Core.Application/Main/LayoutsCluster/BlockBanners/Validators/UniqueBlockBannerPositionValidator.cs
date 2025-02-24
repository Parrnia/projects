using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Validators;

public record UniqueBlockBannerPositionValidator : IRequest<bool>
{
    public BlockBannerPosition BlockBannerPosition { get; init; }
    public int BlockBannerId { get; init; }
}

public class UniqueRoleIdInIdentityServerValidatorHandler : IRequestHandler<UniqueBlockBannerPositionValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueRoleIdInIdentityServerValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueBlockBannerPositionValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.BlockBanners.Where(c => c.Id != request.BlockBannerId)
            .AllAsync(e => e.BlockBannerPosition != request.BlockBannerPosition, cancellationToken);
        return isUnique;
    }
}
