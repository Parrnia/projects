using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum BlockBannerPosition
{
    [Display(Name = "سمت چپ")]
    LeftSide = 0,
    [Display(Name = "سمت راست")]
    RightSide = 1,
}
