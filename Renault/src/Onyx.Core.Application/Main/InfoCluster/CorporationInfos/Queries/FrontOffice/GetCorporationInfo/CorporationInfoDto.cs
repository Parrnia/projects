using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.FrontOffice.GetCorporationInfo;
public class CorporationInfoDto : IMapFrom<CorporationInfo>
{
    public int Id { get; set; }
    public string ContactUsMessage { get; set; } = null!;
    public List<string> PhoneNumbers { get; set; } = new List<string>();
    public List<string> EmailAddresses { get; set; } = new List<string>();
    public List<string> LocationAddresses { get; set; } = new List<string>();
    public List<string> WorkingHours { get; set; } = new List<string>();
    public string PoweredBy { get; set; } = null!;
    public string CallUs { get; set; } = null!;
    public Guid DesktopLogo { get; set; }
    public Guid MobileLogo { get; set; }
    public Guid FooterLogo { get; set; }
    public Guid SliderBackGroundImage { get; set; }
    public bool IsDefault { get; set; }
}