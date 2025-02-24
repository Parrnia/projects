using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;

namespace Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetAllLinks;
public class AllLinkDto : IMapFrom<Link>
{
    public AllLinkDto()
    {
        ChildLinks = new List<ChildLinkDto>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid? Image { get; set; }
    public int RelatedProductCategoryId { get; set; }
    public ParentLinkDto? ParentLink { get; set; }
    public List<ChildLinkDto>? ChildLinks { get; set; }
}
public class ParentLinkDto : IMapFrom<Link>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid? Image { get; set; }
    public int RelatedProductCategoryId { get; set; }
    public ParentLinkDto? ParentLink { get; set; }
}
public class ChildLinkDto : IMapFrom<Link>
{
    public ChildLinkDto()
    {
        ChildLinks = new List<ChildLinkDto>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid? Image { get; set; }
    public int RelatedProductCategoryId { get; set; }
    public List<ChildLinkDto>? ChildLinks { get; set; }
}

