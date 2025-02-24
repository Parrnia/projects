namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

public class ProductTypeAttributeGroupAttribute : BaseAuditableEntity
{
    public ProductTypeAttributeGroupAttribute(string value)
    {
        Value = value;
    }
    /// <summary>
    /// مقدار
    /// </summary>
    public string Value { get; private set; } = null!;
    /// <summary>
    /// گروه بندی نوع ویژگی محصول
    /// </summary>
    public ProductTypeAttributeGroup ProductTypeAttributeGroup { get; private set; } = null!;
    public int ProductTypeAttributeGroupId { get; private set; }

    public bool SetValue(string value)
    {
        Value = value;
        foreach (var product in ProductTypeAttributeGroup.ProductAttributeTypes.SelectMany(c => c.Products))
        {
            var attribute = product.Attributes.SingleOrDefault(c => c.Name == Value);
            if (attribute != null)
            {
                attribute.Name = Value;
                attribute.Slug = Value.ToLower().Replace(' ', '-');
            }
        }
        return true;
    }

    public bool SetProductTypeAttributeGroup(ProductTypeAttributeGroup productTypeAttributeGroup)
    {
        var attributes = new List<ProductTypeAttributeGroupAttribute> {this};
        ProductTypeAttributeGroup.RemoveAttributes(attributes);

        ProductTypeAttributeGroup = productTypeAttributeGroup;
        ProductTypeAttributeGroupId = productTypeAttributeGroup.Id;

        ProductTypeAttributeGroup.AddAttributes(attributes);

        return true;
    }
}