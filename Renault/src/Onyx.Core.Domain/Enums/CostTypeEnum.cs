using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum CostTypeEnum
{
    [Display(Name = "کالا")]
    Part = 0,
    [Display(Name = "ارسال")]
    Delivery = 1
}
