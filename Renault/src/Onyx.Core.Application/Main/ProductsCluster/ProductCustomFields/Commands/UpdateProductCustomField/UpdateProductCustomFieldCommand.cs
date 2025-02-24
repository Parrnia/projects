using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductCustomFields.Commands.UpdateProductCustomField;
public record UpdateProductCustomFieldCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductId { get; init; }
}

public class UpdateProductCustomFieldCommandHandler : IRequestHandler<UpdateProductCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductCustomField), request.Id);
        }

        entity.FieldName = request.FieldName;
        entity.Value = request.Value;
        entity.ProductId = request.ProductId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}