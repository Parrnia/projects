using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum OrderPaymentType
{
    [Display(Name = "نامشخص")]
    Unspecified = 0,
    [Display(Name = "نقدی")]
    Cash = 1,
    [Display(Name = "اعتباری")]
    Credit = 2,
    [Display(Name = "آنلاین")]
    Online = 3,
    [Display(Name = "اعتباری-آنلاین")]
    CreditOnline = 4,
}
