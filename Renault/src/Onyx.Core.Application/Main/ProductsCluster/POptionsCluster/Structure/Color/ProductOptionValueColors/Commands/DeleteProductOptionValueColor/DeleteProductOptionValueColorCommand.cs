using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.DeleteProductOptionValueColor;

public record DeleteProductOptionValueColorCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionValueColorCommandHandler : IRequestHandler<DeleteProductOptionValueColorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionValueColorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionValueColorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionValueColors
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionValueColor), request.Id);
        }

        _context.ProductOptionValueColors.Remove(entity);

        var products = await _context.Products
            .Where(c => c.ColorOptionId == request.Id)
            .Include(c => c.Attributes).ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == entity.Name);
            if (dbAttribute != null)
            {
                _context.ProductAttributes.Remove(dbAttribute);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}