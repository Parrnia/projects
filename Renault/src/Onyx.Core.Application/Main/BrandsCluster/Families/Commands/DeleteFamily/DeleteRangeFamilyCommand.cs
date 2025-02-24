using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Commands.DeleteFamily;

public class DeleteRangeFamilyCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeFamilyCommandHandler : IRequestHandler<DeleteRangeFamilyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeFamilyCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var entity = await _context.Families
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Family), id);
            }

            _context.Families.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
