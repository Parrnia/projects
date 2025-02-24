using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum OrderStatus
{
    [Display(Name = "در انتظار ثبت")]
    PendingForRegister = 0,
    [Display(Name = "ثبت شده")]
    OrderRegistered = 1,
    [Display(Name = "پرداخت تایید شده")]
    OrderPaymentConfirmed = 2,
    [Display(Name = "سفارش تایید شده")]
    OrderConfirmed = 3,
    [Display(Name = "سفارش جمع آوری شده")]
    OrderPrepared = 4,
    [Display(Name = "ارسال شده")]
    OrderShipped = 5,
    [Display(Name = "پرداخت تایید نشده")]
    OrderPaymentUnconfirmed = 6,
    [Display(Name = "سفارش تایید نشده")]
    OrderUnconfirmed = 7,
    [Display(Name = "سفارش لغو شده")]
    OrderCanceled = 8,
    [Display(Name = "مبلغ سفارش بازگشت داده شده")]
    OrderCostRefunded = 9,
    [Display(Name = "فرایند سفارش کامل شده است")]
    OrderCompleted = 10
}
