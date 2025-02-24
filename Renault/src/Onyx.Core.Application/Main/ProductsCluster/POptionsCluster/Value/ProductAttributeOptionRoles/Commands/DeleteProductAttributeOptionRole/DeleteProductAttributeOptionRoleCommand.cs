using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.DeleteProductAttributeOptionRole;

public record DeleteProductAttributeOptionRoleCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeOptionRoleCommandHandler : IRequestHandler<DeleteProductAttributeOptionRoleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeOptionRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeOptionRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeOptionRoles
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOptionRole), request.Id);
        }

        _context.ProductAttributeOptionRoles.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}