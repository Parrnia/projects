using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;

public class ReturnOrderTotalDocumentDto : IMapFrom<ReturnOrderTotalDocument>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public string Description { get; set; } = null!;

}