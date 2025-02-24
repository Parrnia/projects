using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.CreateTeamMember;
public record CreateTeamMemberCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Position { get; init; } = null!;
    public Guid Avatar { get; init; }
    public bool IsActive { get; init; }
    public int AboutUsId { get; init; }
}

public class CreateTeamMemberCommandHandler : IRequestHandler<CreateTeamMemberCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateTeamMemberCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = new TeamMember()
        {
            Name = request.Name,
            Position = request.Position,
            Avatar = request.Avatar,
            AboutUsId = request.AboutUsId,
            IsActive =  request.IsActive
        };

        await _fileStore.SaveStoredFile(
            request.Avatar,
            FileCategory.TeamMember.ToString(),
            FileCategory.TeamMember,
            null,
            false,
            cancellationToken);


        _context.TeamMembers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
