using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.UpdateProductAttributeType;
public record UpdateProductAttributeTypeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateProductAttributeTypeCommandHandler : IRequestHandler<UpdateProductAttributeTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeType), request.Id);
        }
        //var attributeGroups = new List<ProductTypeAttributeGroup>();
        //foreach (var attributeGroupId in request.AttributeGroups)
        //{
        //    var attributeGroup = await _context.ProductTypeAttributeGroups.FindAsync(attributeGroupId, cancellationToken) ?? throw new NotFoundException(nameof(ProductTypeAttributeGroup), attributeGroupId);
        //    attributeGroups.Add(attributeGroup);
        //}

        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');
        //entity.AttributeGroups = attributeGroups;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}