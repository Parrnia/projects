namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice;
public class FilteredTagDto
{
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
    public int Count { get; set; }
}
