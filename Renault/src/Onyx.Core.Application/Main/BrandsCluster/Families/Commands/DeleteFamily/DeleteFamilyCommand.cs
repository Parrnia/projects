using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Commands.DeleteFamily;
public record DeleteFamilyCommand(int Id) : IRequest<Unit>;

public class DeleteFamilyCommandHandler : IRequestHandler<DeleteFamilyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Families
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Family), request.Id);
        }

        _context.Families.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
