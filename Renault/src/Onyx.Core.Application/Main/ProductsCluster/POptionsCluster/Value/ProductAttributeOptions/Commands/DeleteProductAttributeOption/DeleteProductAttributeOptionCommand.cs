using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.DeleteProductAttributeOption;

public record DeleteProductAttributeOptionCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeOptionCommandHandler : IRequestHandler<DeleteProductAttributeOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeOptions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.Id);
        }

        _context.ProductAttributeOptions.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}