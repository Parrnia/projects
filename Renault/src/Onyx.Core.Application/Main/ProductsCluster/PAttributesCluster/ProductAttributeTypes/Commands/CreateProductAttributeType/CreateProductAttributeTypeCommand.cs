using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.CreateProductAttributeType;
public record CreateProductAttributeTypeCommand : IRequest<int>
{ 
    public string Name { get; init; } = null!;
}

public class CreateProductAttributeTypeCommandHandler : IRequestHandler<CreateProductAttributeTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductAttributeType()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-')
        };

        _context.ProductAttributeTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
