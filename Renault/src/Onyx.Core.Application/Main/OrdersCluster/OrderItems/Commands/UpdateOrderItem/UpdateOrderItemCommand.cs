using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;
///////////////////////////////////////////////todo:Modify this command for use
namespace Onyx.Application.Main.OrdersCluster.OrderItems.Commands.UpdateOrderItem;
public record UpdateOrderItemCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public decimal Price { get; init; }
    public double DiscountPercentForProduct { get; init; }
    public double Quantity { get; init; }
    public int OrderId { get; init; }
    public int ProductAttributeOptionId { get; init; }
    public CustomerTypeEnum CustomerTypeEnum { get; init; }
}

public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderItem), request.Id);
        }


        if (entity.ProductAttributeOptionId == request.ProductAttributeOptionId)
        {
            var mainProductAttributeOption =
                await _context.ProductAttributeOptions.SingleOrDefaultAsync(
                    e => e.Id == request.ProductAttributeOptionId, cancellationToken);
            if (mainProductAttributeOption != null)
            {

                mainProductAttributeOption.Return(entity.Quantity);
                var successfulSale = mainProductAttributeOption.Sell(request.Quantity, request.CustomerTypeEnum);
                if (!successfulSale)
                {
                    throw new OrderException();
                }
            }
            else
            {
                throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);
            }
        }
        else
        {
            var productAttributeOptionOld = await _context.ProductAttributeOptions
                .SingleOrDefaultAsync(e => e.Id == entity.ProductAttributeOptionId, cancellationToken);
            var productAttributeOptionNew =
                await _context.ProductAttributeOptions
                    .SingleOrDefaultAsync(e => e.Id == request.ProductAttributeOptionId, cancellationToken);
            if (productAttributeOptionOld != null && productAttributeOptionNew != null)
            {
                productAttributeOptionOld.Return(entity.Quantity);
                var successfulSale = productAttributeOptionNew.Sell(request.Quantity, request.CustomerTypeEnum);
                if (!successfulSale)
                {
                    throw new OrderException();
                }
            }
            else
            {
                throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId+ " or " + entity.ProductAttributeOptionId);
            }
        }


        entity.Price = request.Price;
        entity.DiscountPercentForProduct = request.DiscountPercentForProduct;
        entity.Quantity = request.Quantity;
        entity.OrderId = request.OrderId;
        entity.ProductAttributeOptionId = request.ProductAttributeOptionId;
        entity.Total = request.Price * (decimal) request.Quantity;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
