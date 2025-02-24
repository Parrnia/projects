using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ApiType
{
    [Display(Name = "گرفتن داده")]
    Get = 0,
    [Display(Name = "ثبت داده")]
    Post = 1,
    [Display(Name = "ویرایش داده")]
    Put = 2,
    [Display(Name = "حذف داده")]
    Delete = 3,
}
