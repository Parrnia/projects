using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.DeleteTeamMember;

public record DeleteTeamMemberCommand(int Id) : IRequest<Unit>;

public class DeleteTeamMemberCommandHandler : IRequestHandler<DeleteTeamMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteTeamMemberCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TeamMembers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TeamMember), request.Id);
        }

        _context.TeamMembers.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}