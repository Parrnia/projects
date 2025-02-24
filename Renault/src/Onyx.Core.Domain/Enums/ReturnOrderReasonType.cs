using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderReasonType
{
    [Display(Name = "سمت مشتری")]
    CustomerSide = 0,
    [Display(Name = "سمت سازمان")]
    OrganizationSide = 1,
}
