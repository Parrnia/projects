using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Queries.FrontOffice.GetAboutUs;
public class AboutUsDto : IMapFrom<AboutUs>
{
    public int Id { get; set; }
    public string TextContent { get; set; } = null!;
    public Guid ImageContent { get; set; }
    public bool IsDefault { get; set; }
}
