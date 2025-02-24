using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Commands.DeleteProvider;

public record DeleteProviderCommand(int Id) : IRequest<Unit>;

public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Providers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Provider), request.Id);
        }

        _context.Providers.Remove(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}