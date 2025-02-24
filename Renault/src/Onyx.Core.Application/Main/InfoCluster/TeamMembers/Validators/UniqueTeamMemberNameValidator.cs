using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Validators;

public record UniqueTeamMemberNameValidator : IRequest<bool>
{
    public int TeamMemberId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueTeamMemberNameValidatorHandler : IRequestHandler<UniqueTeamMemberNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueTeamMemberNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueTeamMemberNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.TeamMembers.Where(c => c.Id != request.TeamMemberId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
