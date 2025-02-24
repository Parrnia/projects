using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.UpdateTeamMember;
public record UpdateTeamMemberCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Position { get; init; } = null!;
    public Guid Avatar { get; init; }
    public bool IsActive { get; init; }
    public int AboutUsId { get; init; }
}

public class UpdateTeamMemberCommandHandler : IRequestHandler<UpdateTeamMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateTeamMemberCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TeamMembers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TeamMember), request.Id);
        }

        entity.Name = request.Name;
        entity.Position = request.Position;
        entity.AboutUsId = request.AboutUsId;
        entity.IsActive = request.IsActive;

        if (entity.Avatar != request.Avatar)
        {
            await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.Avatar,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Avatar = request.Avatar;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}