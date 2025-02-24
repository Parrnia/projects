namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice;
public class FilteredCustomerTicketDto
{
    public List<CustomerTicketDto> CustomerTickets { get; set; } = new List<CustomerTicketDto>();
    public int Count { get; set; }
}
