using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.DeleteProductOptionColor;



public record DeleteProductOptionColorCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionColorCommandHandler : IRequestHandler<DeleteProductOptionColorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionColorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionColors
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionColor), request.Id);
        }

        _context.ProductOptionColors.Remove(entity);

        var attributeGroup = await _context.ProductTypeAttributeGroups
            .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

        if (attributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
        }

        _context.ProductTypeAttributeGroups.Remove(attributeGroup);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}