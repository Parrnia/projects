using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupCustomFields.Commands.CreateProductTypeAttributeGroupCustomField;
public record CreateProductTypeAttributeGroupCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductTypeAttributeGroupId { get; init; }
}

public class CreateProductTypeAttributeGroupCustomFieldCommandHandler : IRequestHandler<CreateProductTypeAttributeGroupCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductTypeAttributeGroupCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductTypeAttributeGroupCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductTypeAttributeGroupCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductTypeAttributeGroupId = request.ProductTypeAttributeGroupId
        };

        _context.ProductTypeAttributeGroupCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
