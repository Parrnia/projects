using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ProductOptionTypeEnum
{
    [Display(Name = "رنگ")]
    Color = 1,
    [Display(Name = "جنس")]
    Material = 2,
    [Display(Name = "تامین کننده")]
    Provider = 3,
}