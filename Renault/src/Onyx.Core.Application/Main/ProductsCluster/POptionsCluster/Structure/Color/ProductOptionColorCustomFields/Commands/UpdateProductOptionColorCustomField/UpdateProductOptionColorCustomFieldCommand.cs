using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColorCustomFields.Commands.UpdateProductOptionColorCustomField;
public record UpdateProductOptionColorCustomFieldCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductOptionColorId { get; init; }
}

public class UpdateProductOptionColorCustomFieldCommandHandler : IRequestHandler<UpdateProductOptionColorCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionColorCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionColorCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionColorCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionColorCustomField), request.Id);
        }

        entity.FieldName = request.FieldName;
        entity.Value = request.Value;
        entity.ProductOptionColorId = request.ProductOptionColorId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}