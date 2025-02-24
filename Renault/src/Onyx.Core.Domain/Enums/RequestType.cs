using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum RequestType
{
    [Display(Name = "گرفتن برند")]
    Brand = 0,
    [Display(Name = "گرفتن خانوداه های خودرو")]
    VehicleType = 1,
    [Display(Name = "گرفتن مدل های خودرو")]
    Vehicle = 2,
    [Display(Name = "گرفتن نوع های خودرو")]
    VehicleModel = 3,
    [Display(Name = "گرفتن دسته بندی قطعات")]
    PartGroup = 4,
    [Display(Name = "گرفتن کشورها")]
    Country = 5,
    [Display(Name = "گرفتن نوع واحدهای اندازه گیری")]
    CountingUnitType = 6,
    [Display(Name = "گرفتن واحدهای اندازه گیری")]
    CountingUnit = 7,
    [Display(Name = "گرفتن وضعیت های محصول")]
    PartStatus = 8,
    [Display(Name = "گرفتن نوع های قطعات")]
    PartType = 9,
    [Display(Name = "گرفتن تامین کنندگان")]
    Provider = 10,
    [Display(Name = "گرفتن قطعات")]
    Part = 11,
    [Display(Name = "گرفتن ارتباط بین قطعات و نوع ها")]
    PartVehicleModel = 12,
    [Display(Name = "به روز کردن قیمت قطعات ")]
    PartPrice = 13,
    [Display(Name = "به روز کردن تعداد قطعات")]
    PartCount = 14,
    [Display(Name = "نوع هزینه")]
    CostType = 15,
    [Display(Name = "گرفتن محصولات به طور کامل")]
    PartFull = 16,
    [Display(Name = "گرفتن ارتباط بین نوع ها و محصولات به طور کامل")]
    PartVehicleModelFull = 17,
    [Display(Name = "ثبت سفارش")]
    InsertOrder = 18,
    [Display(Name = "لغو سفارش")]
    CancelOrder = 19,
    [Display(Name = "لغو سفارش باز ارسال شونده")]
    ResendableCancelOrder = 20,
    [Display(Name = "ورود")]
    Authenticate = 21,
    [Display(Name = "گرفتن اطلاعات فاکتور")]
    Invoice = 22
}
