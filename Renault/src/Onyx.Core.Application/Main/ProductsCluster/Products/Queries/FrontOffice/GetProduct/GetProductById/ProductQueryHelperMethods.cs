using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductById;

public static class ProductQueryHelperMethods
{
    public static ProductByIdDto FilterProductByIdDtoForRole(ProductByIdDto product, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in product.AttributeOptions)
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return product;
    }
    public static ProductByIdDto FindSelectedAttributeOptionForProductByIdDto(ProductByIdDto product, CustomerTypeEnum customerTypeEnum)
    {
        ProductAttributeOptionByIdDto? selectedOption = null;
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

