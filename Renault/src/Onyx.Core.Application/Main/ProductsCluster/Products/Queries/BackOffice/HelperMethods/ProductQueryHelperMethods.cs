using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.HelperMethods;

public static class ProductQueryHelperMethods
{
    public static List<Product> FilterProductsForRole(List<Product> products, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in products.SelectMany(product => product.AttributeOptions))
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return products;
    }
    public static IQueryable<Product> FilterIQueryableProductsForRole(IQueryable<Product> products, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in products.SelectMany(product => product.AttributeOptions))
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return products;
    }
    public static List<ProductDto> FilterProductDtosForRole(List<ProductDto> products, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in products.SelectMany(product => product.AttributeOptions))
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return products;
    }
    public static ProductDto FilterProductDtoForRole(ProductDto product, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in product.AttributeOptions)
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return product;
    }
    public static List<ProductDto> FindSelectedAttributeOptionForProductDtos(List<ProductDto> products, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var product in products)
        {
            ProductAttributeOptionForProductDto? selectedOption = null;
            foreach (var option in product.AttributeOptions)
            {
                var availability = option.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum)?.Availability;
                if (availability != null && option.IsDefault && availability == AvailabilityEnum.InStock)
                {
                    selectedOption = option;
                    break;
                }
            }
            foreach (var option in product.AttributeOptions)
            {
                var availability = option.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum)?.Availability;
                if (availability is AvailabilityEnum.InStock)
                {
                    selectedOption = option;
                    break;
                }
            }
            if (selectedOption == null && product.AttributeOptions.Count > 0)
            {
                selectedOption = product.AttributeOptions[0];
            }

            product.SelectedProductAttributeOption = selectedOption;
        }
        return products;
    }
    public static ProductDto FindSelectedAttributeOptionForProductDto(ProductDto product, CustomerTypeEnum customerTypeEnum)
    {
        ProductAttributeOptionForProductDto? selectedOption = null;
        foreach (var option in product.AttributeOptions)
        {
            var availability = option.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum)?.Availability;
            if (availability != null && option.IsDefault && availability == AvailabilityEnum.InStock)
            {
                selectedOption = option;
                break;
            }
        }
        foreach (var option in product.AttributeOptions)
        {
            var availability = option.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum)?.Availability;
            if (availability is AvailabilityEnum.InStock)
            {
                selectedOption = option;
                break;
            }
        }
        if (selectedOption == null && product.AttributeOptions.Count > 0)
        {
            selectedOption = product.AttributeOptions[0];
        }

        product.SelectedProductAttributeOption = selectedOption;

        return product;
    }
}

