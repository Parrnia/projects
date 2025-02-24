using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeCustomFields.Commands.CreateProductAttributeCustomField;
public record CreateProductAttributeCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductAttributeId { get; init; }
}

public class CreateProductAttributeCustomFieldCommandHandler : IRequestHandler<CreateProductAttributeCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductAttributeCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductAttributeId = request.ProductAttributeId
        };

        _context.ProductAttributeCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
