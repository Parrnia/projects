using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.CreateProductAttributeOptionValue;
public record CreateProductAttributeOptionValueCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
}

public class CreateProductAttributeOptionValueCommandHandler : IRequestHandler<CreateProductAttributeOptionValueCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeOptionValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeOptionValueCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductAttributeOptionValue()
        {
            Name = request.Name,
            Value = request.Value,
        };

        _context.ProductAttributeOptionValues.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
