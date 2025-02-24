using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum OrderTotalType
{
    [Display(Name = "ارسال")]
    Shipping = 0,
    [Display(Name = "مالیات")]
    Tax = 1,
    [Display(Name = "تخفیف نوع مشتری")]
    DiscountOnCustomerType = 2,
    [Display(Name = "تخفیف محصول")]
    DiscountOnProduct = 3,
    [Display(Name = "سایر")]
    Other = 4
}
