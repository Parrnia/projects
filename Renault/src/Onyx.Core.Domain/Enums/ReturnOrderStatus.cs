using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderStatus
{
    [Display(Name = "در انتظار ثبت درخواست")]
    PendingForRegister = 0,
    [Display(Name = "در انتظار بررسی درخواست")]
    Registered = 1,
    [Display(Name = "رد شده")]
    Rejected = 2,
    [Display(Name = "در انتظار ارسال یا جمع آوری کالاهای مرجوعی")]
    Accepted = 3,
    [Display(Name = "در انتظار دریافت")]
    Sent = 4,
    [Display(Name = "در انتظار بررسی کالاها")]
    Received = 5,
    [Display(Name = "همه کالاها تایید شده و در انتظار بازگشت وجه")]
    AllConfirmed = 6,
    [Display(Name = "بازگشت وجه داده شده")]
    CostRefunded = 7,
    [Display(Name = "همه کالاها تایید شده و در انتظار بازگشت وجه احتمالی")]
    SomeConfirmed = 8,
    [Display(Name = "لغو شده")]
    Canceled = 9,
    [Display(Name = "فرایند سفارش بازگشت کامل شده است")]
    Completed = 10
}
