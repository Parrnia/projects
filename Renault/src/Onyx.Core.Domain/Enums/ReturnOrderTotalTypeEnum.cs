using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderTotalType
{
    [Display(Name = "ارسال")]
    Shipping = 0,
    [Display(Name = "بازگشت")]
    ReturnShipping = 1,
    [Display(Name = "ارسال دوباره")]
    ShippingAgain = 2,
    [Display(Name = "مالیات")]
    Tax = 3,
    [Display(Name = "تخفیف کل")]
    TotalDiscount = 4,
    [Display(Name = "سایر")]
    Other = 5
}
