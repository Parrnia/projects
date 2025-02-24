using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.CreateProductTypeAttributeGroup;
public record CreateProductTypeAttributeGroupCommand : IRequest<int>
{ 
    public string Name { get; init; } = null!;
}

public class CreateProductTypeAttributeGroupCommandHandler : IRequestHandler<CreateProductTypeAttributeGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductTypeAttributeGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductTypeAttributeGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductTypeAttributeGroup()
        {
            Name = request.Name,
            Slug = request.Name.ToLower().Replace(' ', '-'),
        };

        _context.ProductTypeAttributeGroups.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
