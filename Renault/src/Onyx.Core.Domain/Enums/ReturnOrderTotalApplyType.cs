using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderTotalApplyType
{
    [Display(Name = "کم کردن از کل")]
    ReduceFromTotal = 0,
    [Display(Name = "اضافه کردن به کل")]
    AddToTotal = 1,
    [Display(Name = "نادیده گیرفتن")]
    DoNothing = 2,
}
