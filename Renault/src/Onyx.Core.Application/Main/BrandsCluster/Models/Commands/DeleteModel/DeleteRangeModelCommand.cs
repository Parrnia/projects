using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.DeleteModel;

public class DeleteRangeModelCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeModelCommandHandler : IRequestHandler<DeleteRangeModelCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeModelCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Models
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Model), id);
            }

            _context.Models.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
