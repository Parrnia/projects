using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Commands.DeleteOrderItem;
public record DeleteOrderItemCommand(int Id) : IRequest<Unit>;

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderItem), request.Id);
        }

        var productAttributeOption =
            await _context.ProductAttributeOptions.SingleOrDefaultAsync(
                e => e.Id == entity.ProductAttributeOptionId, cancellationToken);
        if (productAttributeOption != null)
        {
            productAttributeOption.Return(entity.Quantity);
        }
        else
        {
            throw new NotFoundException(nameof(ProductAttributeOption), entity.ProductAttributeOptionId);
        }

        _context.OrderItems.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}