using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.RequestsCluster;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Commands.DeleteRequest;

public class DeleteRangeRequestLogCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeRequestLogCommandHandler : IRequestHandler<DeleteRangeRequestLogCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeRequestLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeRequestLogCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.RequestLogs
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RequestLog), id);
            }

            _context.RequestLogs.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
