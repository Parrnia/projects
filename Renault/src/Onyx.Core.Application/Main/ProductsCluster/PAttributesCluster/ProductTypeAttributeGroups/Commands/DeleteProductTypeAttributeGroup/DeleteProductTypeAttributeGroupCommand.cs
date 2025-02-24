using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.DeleteProductTypeAttributeGroup;

public record DeleteProductTypeAttributeGroupCommand(int Id) : IRequest<Unit>;

public class DeleteProductTypeAttributeGroupCommandHandler : IRequestHandler<DeleteProductTypeAttributeGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductTypeAttributeGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductTypeAttributeGroupCommand request, CancellationToken cancellationToken)
    {
        var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
            .Include(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .Include(c => c.Attributes)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (productTypeAttributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), request.Id);
        }

        productTypeAttributeGroup.HandleRemoveProductTypeAttributeGroup();

        _context.ProductTypeAttributeGroups.Remove(productTypeAttributeGroup);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}