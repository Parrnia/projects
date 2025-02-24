namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

public class ProductTypeAttributeGroup : BaseAuditableEntity
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
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductTypeAttributeGroupCustomField> ProductTypeAttributeGroupCustomFields { get; set; } = new List<ProductTypeAttributeGroupCustomField>();
    /// <summary>
    /// ویژگی ها
    /// </summary>
    public List<ProductTypeAttributeGroupAttribute> Attributes { get; set; } = new List<ProductTypeAttributeGroupAttribute>();
    /// <summary>
    /// نوع های ویژگی محصول
    /// </summary>
    public List<ProductAttributeType> ProductAttributeTypes { get; set; } = new List<ProductAttributeType>();

    public bool AddAttributes(List<ProductTypeAttributeGroupAttribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            foreach (var product in ProductAttributeTypes.SelectMany(c => c.Products))
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
            Attributes.Add(attribute);
        }
        
        return true;
    }
    public bool RemoveAttributes(List<ProductTypeAttributeGroupAttribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            foreach (var product in ProductAttributeTypes.SelectMany(c => c.Products))
            {
                product.Attributes = product.Attributes.Where(c => c.Name != attribute?.Value).ToList();
            }
            Attributes.Remove(attribute);
        }
        
        return true;
    }

    public bool HandleRemoveProductTypeAttributeGroup()
    {
        foreach (var product in ProductAttributeTypes.SelectMany(c => c.Products))
        {
            foreach (var attributeGroupAttribute in Attributes)
            {
                var attribute = product.Attributes.SingleOrDefault(c => c.Name == attributeGroupAttribute.Value);
                if (attribute != null)
                {
                    product.Attributes.Remove(attribute);
                }
            }
        }
        return true;
    }
}