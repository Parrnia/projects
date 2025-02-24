using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum PaymentType
{
    [Display(Name = "نقدی")]
    Cash = 0,
    [Display(Name = "اعتباری")]
    Credit = 1,
    [Display(Name = "آنلاین")]
    Online = 2
}
