using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeValueCustomFields.Commands.CreateProductAttributeValueCustomField;
public record CreateProductAttributeValueCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductAttributeId { get; init; }
}

public class CreateProductAttributeValueCustomFieldCommandHandler : IRequestHandler<CreateProductAttributeValueCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeValueCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeValueCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductAttributeValueCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductAttributeId = request.ProductAttributeId
        };

        _context.ProductAttributeValueCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
