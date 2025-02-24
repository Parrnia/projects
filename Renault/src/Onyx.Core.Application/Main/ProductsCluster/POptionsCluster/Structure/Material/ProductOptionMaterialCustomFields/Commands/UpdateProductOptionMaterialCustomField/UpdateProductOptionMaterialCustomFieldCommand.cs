using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterialCustomFields.Commands.UpdateProductOptionMaterialCustomField;
public record UpdateProductOptionMaterialCustomFieldCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductOptionMaterialId { get; init; }
}

public class UpdateProductOptionMaterialCustomFieldCommandHandler : IRequestHandler<UpdateProductOptionMaterialCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionMaterialCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionMaterialCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionMaterialCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionMaterialCustomField), request.Id);
        }

        entity.FieldName = request.FieldName;
        entity.Value = request.Value;
        entity.ProductOptionMaterialId = request.ProductOptionMaterialId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}