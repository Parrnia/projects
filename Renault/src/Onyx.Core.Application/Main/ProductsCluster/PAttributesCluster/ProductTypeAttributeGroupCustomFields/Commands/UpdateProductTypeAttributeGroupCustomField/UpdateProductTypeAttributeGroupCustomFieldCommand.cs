using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupCustomFields.Commands.UpdateProductTypeAttributeGroupCustomField;
public record UpdateProductTypeAttributeGroupCustomFieldCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductTypeAttributeGroupId { get; init; }
}

public class UpdateProductTypeAttributeGroupCustomFieldCommandHandler : IRequestHandler<UpdateProductTypeAttributeGroupCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductTypeAttributeGroupCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductTypeAttributeGroupCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypeAttributeGroupCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroupCustomField), request.Id);
        }

        entity.FieldName = request.FieldName;
        entity.Value = request.Value;
        entity.ProductTypeAttributeGroupId = request.ProductTypeAttributeGroupId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}