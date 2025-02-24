using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductCustomFields.Commands.DeleteProductCustomField;

public record DeleteProductCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductCustomFieldCommandHandler : IRequestHandler<DeleteProductCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductCustomField), request.Id);
        }

        _context.ProductCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}