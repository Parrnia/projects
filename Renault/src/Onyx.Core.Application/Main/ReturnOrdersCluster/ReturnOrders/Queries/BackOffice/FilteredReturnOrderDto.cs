namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice;
public class FilteredReturnOrderDto
{
    public List<ReturnOrderDto> ReturnOrders { get; set; } = new List<ReturnOrderDto>();
    public int Count { get; set; }
}
