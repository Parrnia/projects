using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColorCustomFields.Commands.CreateProductOptionColorCustomField;
public record CreateProductOptionColorCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductOptionColorId { get; init; }
}

public class CreateProductOptionColorCustomFieldCommandHandler : IRequestHandler<CreateProductOptionColorCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductOptionColorCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductOptionColorCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductOptionColorCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductOptionColorId = request.ProductOptionColorId
        };

        _context.ProductOptionColorCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
