using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.DeleteBadge;

public record DeleteBadgeCommand(int Id) : IRequest<Unit>;

public class DeleteBadgeCommandHandler : IRequestHandler<DeleteBadgeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteBadgeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBadgeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Badges
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Badge), request.Id);
        }

        _context.Badges.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}