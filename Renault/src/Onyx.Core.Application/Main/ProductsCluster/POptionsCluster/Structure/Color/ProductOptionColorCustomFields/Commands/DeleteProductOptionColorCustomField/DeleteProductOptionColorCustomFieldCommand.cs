using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColorCustomFields.Commands.DeleteProductOptionColorCustomField;

public record DeleteProductOptionColorCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionColorCustomFieldCommandHandler : IRequestHandler<DeleteProductOptionColorCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionColorCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionColorCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionColorCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionColorCustomField), request.Id);
        }

        _context.ProductOptionColorCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}