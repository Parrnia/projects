using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.UpdateProductTypeAttributeGroupAttribute;
public record UpdateProductTypeAttributeGroupAttributeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Value { get; init; } = null!;
    public int ProductTypeAttributeGroupId { get; init; }
}

public class UpdateProductTypeAttributeGroupAttributeCommandHandler : IRequestHandler<UpdateProductTypeAttributeGroupAttributeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductTypeAttributeGroupAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductTypeAttributeGroupAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypeAttributeGroupAttributes
            .Include(c => c.ProductTypeAttributeGroup)
            .ThenInclude(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroupAttribute), request.Id);
        }

        var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
            .Include(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .FirstOrDefaultAsync(x => x.Id == request.ProductTypeAttributeGroupId, cancellationToken);

        if (productTypeAttributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), request.ProductTypeAttributeGroupId);
        }

        entity.SetValue(request.Value);
        entity.SetProductTypeAttributeGroup(productTypeAttributeGroup);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}