using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.FrontOffice.GetCustomerTicket;
public class CustomerTicketByIdDto : IMapFrom<CustomerTicket>
{
    public int Id { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; }
    public string CustomerPhoneNumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public bool IsActive { get; set; }
}
