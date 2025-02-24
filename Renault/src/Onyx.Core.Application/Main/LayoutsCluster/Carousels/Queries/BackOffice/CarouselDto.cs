using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice;
public class CarouselDto : IMapFrom<Carousel>
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public Guid DesktopImage { get; set; }
    public Guid MobileImage { get; set; }
    public string? Offer { get; set; }
    public string Title { get; set; } = null!;
    public string Details { get; set; } = null!;
    public string ButtonLabel { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
