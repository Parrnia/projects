using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderTransportationType
{
    [Display(Name = "تعیین نشده")]
    NotDetermined = 0,
    [Display(Name = "ارسال مشتری")]
    CustomerReturn = 1,
    [Display(Name = "ارسال سازمان")]
    OrganizationReturn = 2,
    [Display(Name = "گرفتن کالا در محل")]
    OnLocation = 3
}
