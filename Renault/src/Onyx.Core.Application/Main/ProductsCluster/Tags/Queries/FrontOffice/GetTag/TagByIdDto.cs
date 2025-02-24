using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTag;
public class TagByIdDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public string EnTitle { get; set; } = null!;
    public string FaTitle { get; set; } = null!;
}
