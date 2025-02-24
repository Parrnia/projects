namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
public class FilteredReviewDto
{
    public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    public int Count { get; set; }
}
