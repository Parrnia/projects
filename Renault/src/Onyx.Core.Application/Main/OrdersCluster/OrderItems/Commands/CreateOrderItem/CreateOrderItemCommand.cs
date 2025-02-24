using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Commands.CreateOrderItem;
public record CreateOrderItemCommand : IRequest<int>
{
    public decimal Price { get; init; }
    public double DiscountPercentForProduct { get; init; }
    public double Quantity { get; init; }
    public int OrderId { get; init; }
    public int ProductAttributeOptionId { get; init; }
    public CustomerTypeEnum CustomerTypeEnum { get; init; }
}

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var mainProductAttributeOption = await _context.ProductAttributeOptions
            .Include(c => c.Product).SingleOrDefaultAsync(
                e => e.Id == request.ProductAttributeOptionId, cancellationToken) ?? throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);

        var optionValues = mainProductAttributeOption.OptionValues.Select(optionValue => new OrderItemProductAttributeOptionValue() {Value = optionValue.Value, Name = optionValue.Name,}).ToList();

        var entity = new OrderItem()
        {
            Price = request.Price,
            DiscountPercentForProduct = request.DiscountPercentForProduct,
            Quantity = request.Quantity,
            OrderId = request.OrderId,
            Total = request.Price * (decimal) request.Quantity,
            ProductLocalizedName = mainProductAttributeOption.Product.LocalizedName,
            ProductName = mainProductAttributeOption.Product.Name,
            ProductNo = mainProductAttributeOption.Product.ProductNo,
            ProductAttributeOptionId = request.ProductAttributeOptionId,
            OptionValues = optionValues
        };
        
        

        var successfulSale = mainProductAttributeOption.Sell(request.Quantity, request.CustomerTypeEnum);
        if (!successfulSale)
        {
            throw new OrderException();
        }


        _context.OrderItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
