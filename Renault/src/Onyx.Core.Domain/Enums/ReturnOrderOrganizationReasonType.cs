using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ReturnOrderOrganizationReasonType
{
    [Display(Name = "کالای اشتباه")]
    WrongProduct = 0,
    [Display(Name = "کالای معیوب")]
    DefectiveProduct = 1,
    [Display(Name = "کالای ناقص")]
    IncompleteProduct = 2,
    [Display(Name = "سایر")]
    Other = 3,
}
