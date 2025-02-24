using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum AvailabilityEnum
{
    [Display(Name = "موجود")]
    InStock = 0,
    [Display(Name = "ناموجود")]
    OutOfStock = 1
}
