namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class FilteredProductDto
{
    public List<BackOfficeProductDto> Products { get; set; } = new List<BackOfficeProductDto>();
    public int Count { get; set; }
}
