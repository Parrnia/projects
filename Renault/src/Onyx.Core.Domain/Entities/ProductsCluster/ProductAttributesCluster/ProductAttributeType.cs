namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
public class ProductAttributeType : BaseAuditableEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string Slug { get; set; } = null!;
    /// <summary>
    /// نوع های گروه بندی ویژگی محصول
    /// </summary>
    public List<ProductTypeAttributeGroup> AttributeGroups { get; set; } = new List<ProductTypeAttributeGroup>();
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();

    public bool AddAttributeGroups(List<ProductTypeAttributeGroup> productTypeAttributeGroups)
    {
        foreach (var productTypeAttributeGroup in productTypeAttributeGroups)
        {
            if (!productTypeAttributeGroups.Contains(productTypeAttributeGroup))
            {
                AttributeGroups.Add(productTypeAttributeGroup);
            }
        }

        foreach (var product in Products)
        {
            foreach (var attribute in productTypeAttributeGroups.SelectMany(c => c.Attributes))
            {
                product.Attributes.Add(new ProductAttribute()
                {
                    Name = attribute.Value,
                    Slug = attribute.Value.ToLower().Replace(' ', '-'),
                    ValueName = "",
                    ValueSlug = "",
                    Featured = false
                });
            }
        }
        return true;
    }

    public bool RemoveAttributeGroups(List<ProductTypeAttributeGroup> productTypeAttributeGroups)
    {
        foreach (var attributeGroup in productTypeAttributeGroups)
        {
            if (AttributeGroups.Contains(attributeGroup))
            {
                AttributeGroups.Remove(attributeGroup);
            }
        }

        foreach (var product in Products)
        {
            foreach (var attribute in productTypeAttributeGroups.SelectMany(c => c.Attributes))
            {
                var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == attribute.Value);
                if (dbAttribute != null)
                {
                    product.Attributes.Remove(dbAttribute);
                }
            }
        }
        return true;
    }

    public bool HandleRemoveProductAttributeType()
    {
        foreach (var product in Products)
        {
            foreach (var attribute in AttributeGroups.SelectMany(c => c.Attributes))
            {
                var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == attribute.Value);
                if (dbAttribute != null)
                {
                    product.Attributes.Remove(dbAttribute);
                }
            }
        }
        return true;
    }
}