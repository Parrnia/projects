using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.DeleteTeamMember;

public class DeleteRangeTeamMemberCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeTeamMemberCommandHandler : IRequestHandler<DeleteRangeTeamMemberCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;


    public DeleteRangeTeamMemberCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeTeamMemberCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.TeamMembers
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TeamMember), id);
            }

            _context.TeamMembers.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
