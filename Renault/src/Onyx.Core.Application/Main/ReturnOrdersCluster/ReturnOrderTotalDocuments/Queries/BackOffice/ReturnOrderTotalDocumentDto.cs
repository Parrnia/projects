using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.BackOffice;
public class ReturnOrderTotalDocumentDto : IMapFrom<ReturnOrderTotalDocument>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public string Description { get; set; } = null!;
    public int ReturnOrderTotalId { get; set; }
}
