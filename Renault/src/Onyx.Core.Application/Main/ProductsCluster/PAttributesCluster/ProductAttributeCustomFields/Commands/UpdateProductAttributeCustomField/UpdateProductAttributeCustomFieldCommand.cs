using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeCustomFields.Commands.UpdateProductAttributeCustomField;
public record UpdateProductAttributeCustomFieldCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductAttributeId { get; init; }
}

public class UpdateProductAttributeCustomFieldCommandHandler : IRequestHandler<UpdateProductAttributeCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeCustomField), request.Id);
        }

        entity.FieldName = request.FieldName;
        entity.Value = request.Value;
        entity.ProductAttributeId = request.ProductAttributeId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}