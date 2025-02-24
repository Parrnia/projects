using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterialCustomFields.Commands.CreateProductOptionMaterialCustomField;
public record CreateProductOptionMaterialCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductOptionMaterialId { get; init; }
}

public class CreateProductOptionMaterialCustomFieldCommandHandler : IRequestHandler<CreateProductOptionMaterialCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionMaterialCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionMaterialCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionMaterialCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductOptionMaterialId = request.ProductOptionMaterialId
        };

        _context.ProductOptionMaterialCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
