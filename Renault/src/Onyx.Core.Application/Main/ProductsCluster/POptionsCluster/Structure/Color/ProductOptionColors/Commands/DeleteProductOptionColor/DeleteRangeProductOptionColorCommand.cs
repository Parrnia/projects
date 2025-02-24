using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.DeleteProductOptionColor;

public class DeleteRangeProductOptionColorCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeProductOptionColorCommandHandler : IRequestHandler<DeleteRangeProductOptionColorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductOptionColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductOptionColorCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductOptionColors
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductOptionColor), id);
            }

            _context.ProductOptionColors.Remove(entity);


            var attributeGroup = await _context.ProductTypeAttributeGroups
                .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

            if (attributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
            }

            _context.ProductTypeAttributeGroups.Remove(attributeGroup);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
