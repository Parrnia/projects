using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice;
public class BlockBannerDto : IMapFrom<BlockBanner>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Subtitle { get; set; } = null!;
    public string ButtonText { get; set; } = null!;
    public Guid Image { get; set; }
    public bool IsActive { get; set; }
    public BlockBannerPosition BlockBannerPosition { get; set; }
    
    public string BlockBannerPositionName => EnumHelper<BlockBannerPosition>.GetDisplayValue(BlockBannerPosition);
}
