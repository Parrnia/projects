using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.CreateProductAttribute;
public record CreateProductAttributeCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public bool Featured { get; init; }
    public int ProductId { get; init; }
    public string ValueName { get; set; } = null!;
}
public class CreateProductAttributeCommandHandler : IRequestHandler<CreateProductAttributeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductAttribute()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
            Featured = request.Featured,
            ValueName = request.ValueName,
            ValueSlug = request.ValueName.ToLower().Replace(' ', '-'),
            ProductId = request.ProductId,
        };

        _context.ProductAttributes.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
