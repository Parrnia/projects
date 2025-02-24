using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;
public enum OnlinePaymentStatus
{
    [Display(Name = "در انتظار پرداخت")]
    Waiting,

    [Display(Name = "پرداخت شده")]
    Completed,

    [Display(Name = "لغو شده")]
    Failed,

    [Display(Name = "منقضی شده")]
    Expired,
}
