using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice;
public class ReturnOrderItemDocumentDto : IMapFrom<ReturnOrderItemDocument>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public string Description { get; set; } = null!;
    public int ReturnOrderItemId { get; set; }
}
