using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupCustomFields.Commands.DeleteProductTypeAttributeGroupCustomField;

public record DeleteProductTypeAttributeGroupCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductTypeAttributeGroupCustomFieldCommandHandler : IRequestHandler<DeleteProductTypeAttributeGroupCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductTypeAttributeGroupCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductTypeAttributeGroupCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypeAttributeGroupCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroupCustomField), request.Id);
        }

        _context.ProductTypeAttributeGroupCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}