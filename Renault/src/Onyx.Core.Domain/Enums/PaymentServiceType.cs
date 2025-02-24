using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum PaymentServiceType
{
    [Display(Name = "آسان پرداخت")]
    AsanPardakht = 1,
    [Display(Name = "پارسیان")]
    Parsian = 2
}