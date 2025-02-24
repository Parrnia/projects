namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;
public class FilteredOrderDto
{
    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    public int Count { get; set; }
}
