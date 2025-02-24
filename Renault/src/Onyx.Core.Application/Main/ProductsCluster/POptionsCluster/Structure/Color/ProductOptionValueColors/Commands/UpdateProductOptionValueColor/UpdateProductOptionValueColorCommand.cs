using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.UpdateProductOptionValueColor;
public record UpdateProductOptionValueColorCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
    public int ProductOptionColorId { get; init; }
}

public class UpdateProductOptionValueColorCommandHandler : IRequestHandler<UpdateProductOptionValueColorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductOptionValueColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductOptionValueColorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionValueColors
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionValueColor), request.Id);
        }

        


        var products = await _context.Products
            .Where(c => c.ColorOptionId == request.ProductOptionColorId)
            .Include(c => c.Attributes).ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == entity.Name);
            if (dbAttribute != null)
            {
                dbAttribute.Name = request.Name;
                dbAttribute.Slug = request.Name.ToLower().Replace(' ', '-');
            }
        }
        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');
        entity.Color = request.Color;
        entity.ProductOptionColorId = request.ProductOptionColorId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}