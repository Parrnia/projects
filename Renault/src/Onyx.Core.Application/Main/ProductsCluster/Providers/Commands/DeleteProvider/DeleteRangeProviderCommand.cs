using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Commands.DeleteProvider;

public class DeleteRangeProviderCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeProviderCommandHandler : IRequestHandler<DeleteRangeProviderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProviderCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Providers
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Provider), id);
            }

            _context.Providers.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
