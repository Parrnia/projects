using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice;
public class ThemeDto : IMapFrom<Theme>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string BtnPrimaryColor { get; set; } = null!;
    public string BtnPrimaryHoverColor { get; set; } = null!;
    public string BtnSecondaryColor { get; set; } = null!;
    public string BtnSecondaryHoverColor { get; set; } = null!;
    public string ThemeColor { get; set; } = null!;
    public string HeaderAndFooterColor { get; set; } = null!;
    public bool IsDefault { get; set; }
}
