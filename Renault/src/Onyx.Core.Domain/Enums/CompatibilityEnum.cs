using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum CompatibilityEnum
{
    [Display(Name = "سازگار با همه")]
    All = 0,
    [Display(Name = "ناشناخته")]
    Unknown = 1,
    [Display(Name = "سازگار با بعضی محصولات")]
    Compatible = 2
}