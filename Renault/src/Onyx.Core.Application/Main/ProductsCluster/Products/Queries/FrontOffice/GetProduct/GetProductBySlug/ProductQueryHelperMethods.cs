using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductBySlug;

public static class ProductQueryHelperMethods
{
    public static ProductBySlugDto FilterProductBySlugDtoForRole(ProductBySlugDto product, CustomerTypeEnum customerTypeEnum)
    {
        foreach (var productAttributeOption in product.AttributeOptions)
        {
            productAttributeOption.ProductAttributeOptionRoles = productAttributeOption.ProductAttributeOptionRoles
                .Where(c => c.CustomerTypeEnum == customerTypeEnum).ToList();
        }

        return product;
    }
    public static ProductBySlugDto FindSelectedAttributeOptionForProductBySlugDto(ProductBySlugDto product, CustomerTypeEnum customerTypeEnum)
    {
        ProductAttributeOptionBySlugDto? selectedOption = null;
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

