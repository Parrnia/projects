using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice;
public class ReturnOrderItemGroupProductAttributeOptionValueDto : IMapFrom<ReturnOrderItemGroupProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public int ReturnOrderItemGroupId { get; set; }
}
