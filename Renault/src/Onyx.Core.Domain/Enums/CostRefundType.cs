using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum CostRefundType
{
    [Display(Name = "تعیین نشده")]
    NotDetermined = 0,
    [Display(Name = "نقدی")]
    Cash = 1,
    [Display(Name = "اعتباری")]
    Credit = 2,
    [Display(Name = "آنلاین")]
    Online = 3
}
