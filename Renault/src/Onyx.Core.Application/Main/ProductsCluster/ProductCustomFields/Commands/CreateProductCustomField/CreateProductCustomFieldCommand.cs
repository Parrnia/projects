using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductCustomFields.Commands.CreateProductCustomField;
public record CreateProductCustomFieldCommand : IRequest<int>
{
    public string FieldName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int ProductId { get; init; }
}

public class CreateProductCustomFieldCommandHandler : IRequestHandler<CreateProductCustomFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductCustomField()
        {
            FieldName = request.FieldName,
            Value = request.Value,
            ProductId = request.ProductId
        };

        _context.ProductCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
