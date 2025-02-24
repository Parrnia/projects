using Onyx.Application.Main.OrdersCluster.Orders.Commands.CreateOrder.CreateOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Common.Interfaces;
public interface ICreateOrderHelper
{
    public Task CheckAndSyncWithSeven(List<Product> products,
        List<CreateOrderItemCommandForOrder> orderItemCommands, CancellationToken cancellationToken);

    public Task CheckAndSyncWithSeven(List<Product> products,
        List<OrderItem> orderItems, CancellationToken cancellationToken);

    public Task<List<OrderItem>> CheckAndSyncWithSevenAndUpdateOrder(List<Product> products,
        List<OrderItem> orderItems, CustomerTypeEnum customerType, CancellationToken cancellationToken);

    public bool CheckProductsAvailability(List<Product> products,
        List<OrderItem> orderItems, CustomerTypeEnum customerType);

    public decimal CalculateDeliveryCost(decimal orderCost, List<OrderTotal> totals);

    public decimal CalculateTax(decimal cost, double discountPercent);

    public decimal CalculateDiscount(decimal cost, double discountPercent);

    public decimal CalculateTotalOrderPrice(decimal subTotal, List<OrderTotal> totals);

    public Task<string> CreateOrderNumber(CancellationToken cancellationToken);

    public Task<CustomerType> GetCustomerType(CustomerTypeEnum customerTypeEnum,
        CancellationToken cancellationToken);

    public Task<List<Product>> GetProducts(List<int> productIds, CancellationToken cancellationToken);

    public Task<Customer> GetCustomer(Guid customerId, CancellationToken cancellationToken);

    public Task<Country> GetCountry(int countryId, CancellationToken cancellationToken);

    public Product GetProduct(int productId, List<Product> products);

    public ProductAttributeOption GetProductAttributeOption(int productAttributeOptionId,
        List<ProductAttributeOption> productAttributeOptions);

    public ProductAttributeOptionRole GetProductAttributeOptionRole(CustomerTypeEnum customerTypeEnum,
        List<ProductAttributeOptionRole> productAttributeOptionRoles);

    public void CheckRequestAndDatabase(CreateOrderCommand command, Order order);

    public SevenSoftCommand CreateSevenSoftCommand(Order order,
        List<SevenSoftOrderProductCommand> productCommands);
}
