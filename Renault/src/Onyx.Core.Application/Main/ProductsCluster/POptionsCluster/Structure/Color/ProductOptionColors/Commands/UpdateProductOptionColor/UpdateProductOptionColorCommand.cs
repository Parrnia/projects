using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.UpdateProductOptionColor;
public record UpdateProductOptionColorCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateProductOptionColorCommandHandler : IRequestHandler<UpdateProductOptionColorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionColorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionColors
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionColor), request.Id);
        }

        var attributeGroup = await _context.ProductTypeAttributeGroups
            .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

        if (attributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
        }

        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');

        attributeGroup.Name = entity.Name;
        attributeGroup.Slug = entity.Slug;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}