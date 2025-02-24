using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;

namespace Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetFirstLayerLinks;
public class FirstLayerLinkDto : IMapFrom<Link>
{
    public FirstLayerLinkDto()
    {
        ChildLinks = new List<ChildLinkDtoFirstLayer>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid? Image { get; set; }
    public int RelatedProductCategoryId { get; set; }
    public List<ChildLinkDtoFirstLayer>? ChildLinks { get; set; }
}
public class ChildLinkDtoFirstLayer : IMapFrom<Link>
{
    public ChildLinkDtoFirstLayer()
    {
        ChildLinks = new List<ChildLinkDtoSecondLayer>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int RelatedProductCategoryId { get; set; }
    public List<ChildLinkDtoSecondLayer>? ChildLinks { get; set; }
}
public class ChildLinkDtoSecondLayer : IMapFrom<Link>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int RelatedProductCategoryId { get; set; }
    public List<ChildLinkDtoSecondLayer>? ChildLinks { get; set; }
}