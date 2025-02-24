using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

/// <summary>
/// نوع مشتری
/// </summary>
public enum CustomerTypeEnum
{
    [Display(Name = "شخصی")]
    Personal = 1,

    [Display(Name = "فروشگاهی")]
    Store = 2,

    [Display(Name = "نمایندگی")]
    Agency = 3,

    [Display(Name = "تعمیرگاه مرکزی")]
    CentralRepairShop = 4,
}