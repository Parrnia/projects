using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.CreateOrder.CreateOrder;
using Onyx.Application.Services;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;
public class CreateOrderHelper : ICreateOrderHelper
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ApplicationSettings _applicationSettings;

    public CreateOrderHelper(IApplicationDbContext context, ISevenSoftService sevenSoftService, IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
        _applicationSettings = applicationSettings.Value;
    }
    public async Task CheckAndSyncWithSeven(List<Product> products, List<CreateOrderItemCommandForOrder> orderItemCommands, CancellationToken cancellationToken)
    {
        var productPrices = new List<ProductPrice>();
        var productCounts = new List<ProductCount>();


        foreach (var orderItemCommand in orderItemCommands)
        {
            var product = products.SingleOrDefault(d => d.Id == orderItemCommand.ProductId);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), orderItemCommand.ProductId);
            }

            var productAttributeOption = product.AttributeOptions.SingleOrDefault(e => e.Id == orderItemCommand.ProductAttributeOptionId);
            if (productAttributeOption == null)
            {
                throw new NotFoundException(nameof(ProductAttributeOption), orderItemCommand.ProductAttributeOptionId);
            }


            var price = productAttributeOption.GetPrice();
            var count = productAttributeOption.TotalCount;
            var quantity = orderItemCommand.Quantity;

            productPrices.Add(new ProductPrice()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Price = price,
                OrderPrice = price
            });
            productCounts.Add(new ProductCount()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Number = count,
                OrderQuantity = quantity
            });
        }

        var checkPricesResult = await _sevenSoftService.CheckPrices(productPrices, cancellationToken);
        var checkCountsResult = await _sevenSoftService.CheckCounts(productCounts, cancellationToken);

        if (!checkPricesResult.IsValid)
        {
            if (checkPricesResult.ProductPrices != null)
            {
                foreach (var orderItemCommand in orderItemCommands)
                {
                    var product = products.SingleOrDefault(d => d.Id == orderItemCommand.ProductId);
                    var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItemCommand.ProductAttributeOptionId);
                    var price = checkPricesResult.ProductPrices.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Price;
                    productAttributeOption?.SetPrice(price ?? 0);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }


            throw new OrderException("با عرض پوزش، اطلاعات فروشگاه در حال به روزرسانی است. لطفا دقایقی بعد دوباره تلاش نمایید");
        }


        if (checkCountsResult.ProductCounts != null)
        {
            foreach (var orderItemCommand in orderItemCommands)
            {
                var product = products.SingleOrDefault(d => d.Id == orderItemCommand.ProductId);
                var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItemCommand.ProductAttributeOptionId);
                var count = checkCountsResult.ProductCounts.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Number;
                productAttributeOption?.SetTotalCount(count ?? 0);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }



        if (!checkCountsResult.IsValid)
        {
            throw new OrderException("با عرض پوزش، اطلاعات فروشگاه در حال به روزرسانی است. لطفا دقایقی بعد دوباره تلاش نمایید\");\r\n        }");
        }
    }

    public async Task CheckAndSyncWithSeven(List<Product> products, List<OrderItem> orderItems, CancellationToken cancellationToken)
    {
        var productPrices = new List<ProductPrice>();
        var productCounts = new List<ProductCount>();


        foreach (var orderItem in orderItems)
        {
            var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), orderItem.ProductAttributeOption.ProductId);
            }

            var productAttributeOption = product.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
            if (productAttributeOption == null)
            {
                throw new NotFoundException(nameof(ProductAttributeOption), orderItem.ProductAttributeOptionId);
            }


            var price = productAttributeOption.GetPrice();
            var orderPrice = orderItem.Price;

            var count = productAttributeOption.TotalCount;
            var quantity = orderItem.Quantity;

            productPrices.Add(new ProductPrice()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Price = price,
                OrderPrice = orderPrice
            });
            productCounts.Add(new ProductCount()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Number = count,
                OrderQuantity = quantity
            });
        }

        var checkPricesResult = await _sevenSoftService.CheckPrices(productPrices, cancellationToken);
        var checkCountsResult = await _sevenSoftService.CheckCounts(productCounts, cancellationToken);

        if (!checkPricesResult.IsValid)
        {
            if (checkPricesResult.ProductPrices != null)
            {
                foreach (var orderItem in orderItems)
                {
                    var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
                    var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
                    var price = checkPricesResult.ProductPrices.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Price;
                    productAttributeOption?.SetPrice(price ?? 0);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }


            throw new OrderException("قیمت یک یا چند کالا در این سفارش تغییر یافته است برای به روزرسانی سبد خرید بر روی دکمه به روزرسانی کلیک نمایید");
        }


        if (checkCountsResult.ProductCounts != null)
        {
            foreach (var orderItem in orderItems)
            {
                var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
                var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
                var count = checkCountsResult.ProductCounts.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Number;
                productAttributeOption?.SetTotalCount(count ?? 0);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }



        if (!checkCountsResult.IsValid)
        {
            throw new OrderException("موجودی یک یا چند کالا در این سفارش تغییر یافته است برای به روزرسانی سبد خرید بر روی دکمه به روزرسانی کلیک نمایید\");\r\n        }");
        }
    }

    public async Task<List<OrderItem>> CheckAndSyncWithSevenAndUpdateOrder(List<Product> products,
        List<OrderItem> orderItems, CustomerTypeEnum customerType, CancellationToken cancellationToken)
    {
        var productPrices = new List<ProductPrice>();
        var productCounts = new List<ProductCount>();


        foreach (var orderItem in orderItems)
        {
            var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), orderItem.ProductAttributeOption.ProductId);
            }

            var productAttributeOption = product.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
            if (productAttributeOption == null)
            {
                throw new NotFoundException(nameof(ProductAttributeOption), orderItem.ProductAttributeOptionId);
            }


            var price = productAttributeOption.GetPrice();
            var orderPrice = orderItem.Price;

            var count = productAttributeOption.TotalCount;
            var quantity = orderItem.Quantity;

            productPrices.Add(new ProductPrice()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Price = price,
                OrderPrice = orderPrice
            });
            productCounts.Add(new ProductCount()
            {
                PartId = product.Related7SoftProductId ?? Guid.Empty,
                Number = count,
                OrderQuantity = quantity
            });
        }

        var checkPricesResult = await _sevenSoftService.CheckPrices(productPrices, cancellationToken);
        var checkCountsResult = await _sevenSoftService.CheckCounts(productCounts, cancellationToken);

        if (!checkPricesResult.IsValid)
        {
            if (checkPricesResult.ProductPrices != null)
            {
                foreach (var orderItem in orderItems)
                {
                    var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
                    var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
                    var price = checkPricesResult.ProductPrices.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Price;
                    productAttributeOption?.SetPrice(price ?? 0);
                }
            }
        }


        if (checkCountsResult.ProductCounts != null)
        {
            foreach (var orderItem in orderItems)
            {
                var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
                var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
                var count = checkCountsResult.ProductCounts.SingleOrDefault(c => c.PartId == product?.Related7SoftProductId)?.Number;
                productAttributeOption?.SetTotalCount(count ?? 0);
            }
        }

        foreach (var orderItem in orderItems)
        {
            var product = products.SingleOrDefault(d => d.Id == orderItem.ProductAttributeOption.ProductId);
            var productAttributeOption = product?.AttributeOptions.SingleOrDefault(e => e.Id == orderItem.ProductAttributeOptionId);
            var maxOrderQty = productAttributeOption?.ProductAttributeOptionRoles
                .SingleOrDefault(c => c.CustomerTypeEnum == customerType)?.CurrentMaxOrderQty;
            var minOrderQty = productAttributeOption?.ProductAttributeOptionRoles
                .SingleOrDefault(c => c.CustomerTypeEnum == customerType)?.CurrentMinOrderQty;
            if (maxOrderQty != null && orderItem.Quantity > maxOrderQty)
            {
                orderItem.Quantity = (double)maxOrderQty;
            }
            if (minOrderQty != null && orderItem.Quantity < minOrderQty)
            {
                orderItem.Quantity = (double)minOrderQty;
            }
        }

        orderItems = orderItems.Where(c => c.Quantity != 0).ToList();
        return orderItems;
    }

    public bool CheckProductsAvailability(List<Product> products, List<OrderItem> orderItems, CustomerTypeEnum customerType)
    {
        foreach (var attributeOption in products.SelectMany(c => c.AttributeOptions))
        {
            var orderItem = orderItems.SingleOrDefault(c => c.ProductAttributeOptionId == attributeOption.Id);
            if (orderItem != null)
            {
                if (!attributeOption.CheckAvailability(orderItem.Quantity, customerType))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public decimal CalculateDeliveryCost(decimal orderCost, List<OrderTotal> totals)
    {
        var orderTotalDiscountOnProduct = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnProduct);
        if (orderTotalDiscountOnProduct != null)
        {
            orderCost -= orderTotalDiscountOnProduct.Price;
        }
        var orderTotalDiscountOnCustomerType = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnCustomerType);
        if (orderTotalDiscountOnCustomerType != null)
        {
            orderCost -= orderTotalDiscountOnCustomerType.Price;
        }
        switch (orderCost)
        {
            case >= 100000000:
                return 0;
            case >= 50000000:
                return 600000;
            default:
                return 1000000;
        }
    }

    public decimal CalculateTax(decimal cost, double discountPercent)
    {
        var tax = cost * ((decimal)_applicationSettings.TaxPercent / 100) * (1 - (decimal)discountPercent / 100);
        return tax;
    }

    public decimal CalculateDiscount(decimal cost, double discountPercent)
    {
        return cost * (decimal)discountPercent / 100;
    }

    public decimal CalculateTotalOrderPrice(decimal subTotal, List<OrderTotal> totals)
    {
        var orderTotalTax = totals.SingleOrDefault(c => c.Type == OrderTotalType.Tax);
        if (orderTotalTax != null)
        {
            subTotal += orderTotalTax.Price;
        }
        var orderTotalDiscountOnProduct = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnProduct);
        if (orderTotalDiscountOnProduct != null)
        {
            subTotal -= orderTotalDiscountOnProduct.Price;
        }
        var orderTotalDiscountOnCustomerType = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnCustomerType);
        if (orderTotalDiscountOnCustomerType != null)
        {
            subTotal -= orderTotalDiscountOnCustomerType.Price;
        }
        var deliveryOrderTotal = totals.SingleOrDefault(c => c.Type == OrderTotalType.Shipping);
        if (deliveryOrderTotal != null)
        {
            subTotal += deliveryOrderTotal.Price;
        }
        return subTotal;
    }

    public async Task<string> CreateOrderNumber(CancellationToken cancellationToken)
    {
        var todayDate = DateTime.Now.Date.ToPersianDayOnly();
        var lastOrder = await _context.Orders.OrderBy(c => c.Id).LastOrDefaultAsync(cancellationToken);
        var random = new Random();
        random.Next(1, 100);
        var orderNumber = todayDate + '-' + random.Next(0, 10000) + (lastOrder?.Id ?? 0) + '-' + random.Next(0, 100);
        return orderNumber;
    }

    public async Task<CustomerType> GetCustomerType(CustomerTypeEnum customerTypeEnum, CancellationToken cancellationToken)
    {
        return await _context.CustomerTypes.SingleOrDefaultAsync(
            e => e.CustomerTypeEnum == customerTypeEnum, cancellationToken) ?? throw new NotFoundException(nameof(CustomerType), customerTypeEnum);
    }

    public async Task<List<Product>> GetProducts(List<int> productIds, CancellationToken cancellationToken)
    {
        var products = new List<Product>();
        foreach (var productId in productIds)
        {
            var product = await _context.Products
                              .Include(c => c.AttributeOptions).ThenInclude(c => c.OptionValues)
                              .Include(c => c.AttributeOptions).ThenInclude(c => c.ProductAttributeOptionRoles)
                              .Include(c => c.AttributeOptions).ThenInclude(c => c.Prices)
                              .Include(c => c.ColorOption).ThenInclude(e => e.Values)
                              .Include(c => c.MaterialOption).ThenInclude(e => e.Values)
                              .SingleOrDefaultAsync(c => c.Id == productId && c.IsActive == true, cancellationToken)
                          ?? throw new NotFoundException(nameof(Product), productId);
            products.Add(product);
        }
        return products;
    }

    public async Task<Customer> GetCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
                           .Include(c => c.Credits)
                           .SingleOrDefaultAsync(e => e.Id == customerId, cancellationToken)
                       ??
                       throw new NotFoundException(nameof(Customer), customerId);
        return customer;
    }

    public async Task<Country> GetCountry(int countryId, CancellationToken cancellationToken)
    {
        var country = await _context.Countries.SingleOrDefaultAsync(e => e.Id == countryId, cancellationToken)
                      ??
                      throw new NotFoundException(nameof(Country), countryId);
        return country;
    }

    public Product GetProduct(int productId, List<Product> products)
    {
        var product = products.SingleOrDefault(d => d.Id == productId);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), productId);
        }
        return product;
    }
    public ProductAttributeOption GetProductAttributeOption(int productAttributeOptionId, List<ProductAttributeOption> productAttributeOptions)
    {
        var productAttributeOption = productAttributeOptions.SingleOrDefault(e => e.Id == productAttributeOptionId);
        if (productAttributeOption == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), productAttributeOptionId);
        }
        return productAttributeOption;
    }
    public ProductAttributeOptionRole GetProductAttributeOptionRole(CustomerTypeEnum customerTypeEnum, List<ProductAttributeOptionRole> productAttributeOptionRoles)
    {
        var productAttributeOptionRole = productAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum);
        if (productAttributeOptionRole == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOptionRole), customerTypeEnum);
        }
        return productAttributeOptionRole;
    }

    public void CheckRequestAndDatabase(CreateOrderCommand command, Order order)
    {
        if (Math.Abs(command.Quantity - order.Items.Sum(c => c.Quantity)) > 0)
        {
            throw new OrderException("در تعداد محصولات درخواستی مغایرت وجود دارد");
        }
        if (Math.Abs(10 * command.Subtotal - order.Subtotal * (decimal)1.1) > 100)
        {
            throw new OrderException("در جمع قیمت محصولات مغایرت وجود دارد");
        }
        if (Math.Abs(10 * command.Total - order.Total) > 100)
        {
            throw new OrderException("در جمع قیمت کل سفارش مغایرت وجود دارد");
        }
    }

    public SevenSoftCommand CreateSevenSoftCommand(Order order, List<SevenSoftOrderProductCommand> productCommands)
    {
        var sevenSoftCommand = new SevenSoftCommand();
        //todo: change AddSpExchangesVmDescription
        var orderId = order.Id + 90;
        var sevenSoftOrderCommand = new SevenSoftOrderCommand
        {
            AddSpExchangesVmSubscriberName = order.CustomerFirstName + " " + order.CustomerLastName,
            AddSpExchangesVmSubscriberNationalCode = order.NationalCode,
            AddSpExchangesVmSubscriberTel = order.PhoneNumber,
            AddSpExchangesVmDescription = "شماره مرتبط در سایت: " + orderId,
            AddSpExchangesVmDiscountValue = 0,
            AddSpExchangesVmDiscountPercent = 0,
            RelatedCode = order.Number
        };

        var sevenSoftOrderCustomerCommand = new SevenSoftOrderCustomerCommand
        {
            SubscriberFirstName = order.CustomerFirstName,
            SubscriberLastName = order.CustomerLastName,
            Mobile = order.PhoneNumber
        };

        if (order.PersonType == PersonType.Natural)
        {
            sevenSoftOrderCustomerCommand.SubscriberVmBirthCityRelatedCode = 0;
            sevenSoftOrderCustomerCommand.SubscriberVmIssueCityRelatedCode = 0;
            sevenSoftOrderCustomerCommand.IssueDate = DateTime.Now;
            sevenSoftOrderCustomerCommand.FatherName = "";
            sevenSoftOrderCustomerCommand.NationalCode = order.NationalCode;
            sevenSoftOrderCustomerCommand.IdNumber = "";
            sevenSoftOrderCustomerCommand.Gender = 0;
            sevenSoftOrderCustomerCommand.BirthDate = new DateTime(1753, 1, 1);
            sevenSoftOrderCustomerCommand.HomeAddress = order.OrderAddress.AddressDetails1 + order.OrderAddress.AddressDetails2;
            sevenSoftOrderCustomerCommand.HomeTel = order.PhoneNumber;
            sevenSoftOrderCustomerCommand.IsLegalSubscriber = false;

        }
        else if (order.PersonType == PersonType.Legal)
        {
            sevenSoftOrderCustomerCommand.CompanyName = order.CustomerFirstName;
            sevenSoftOrderCustomerCommand.NationalId = order.NationalCode;
            sevenSoftOrderCustomerCommand.NationalCode = order.NationalCode;
            sevenSoftOrderCustomerCommand.EconomicCode = order.CustomerLastName;
            sevenSoftOrderCustomerCommand.ManagerName = "";
            sevenSoftOrderCustomerCommand.OfficeAddress = order.OrderAddress.AddressDetails1 + order.OrderAddress.AddressDetails2;
            sevenSoftOrderCustomerCommand.OfficeTel = order.PhoneNumber;
            sevenSoftOrderCustomerCommand.IsLegalSubscriber = true;
            sevenSoftOrderCustomerCommand.IssueDate = DateTime.Now;
            sevenSoftOrderCustomerCommand.BirthDate = new DateTime(1753, 1, 1);
        }

        sevenSoftOrderCommand.AddSpExchangesVmSpExchangesParts = productCommands;

        sevenSoftCommand.SubscriberComplete = sevenSoftOrderCustomerCommand;
        sevenSoftCommand.AddSpExchanges = sevenSoftOrderCommand;

        return sevenSoftCommand;
    }
}
