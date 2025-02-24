using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;
public enum FileCategory
{
    /// <summary>
    /// برند محصول
    /// </summary>
    [Display(Name = "برند محصول")]
    ProductBrand = 0,
    /// <summary>
    /// برند خودرو
    /// </summary>
    [Display(Name = "برند خودرو")]
    VehicleBrand = 1,
    /// <summary>
    /// دسته بندی بلاگ
    /// </summary>
    [Display(Name = "دسته بندی بلاگ")]
    BlogCategory = 2,
    /// <summary>
    /// دسته بندی محصول
    /// </summary>
    [Display(Name = "دسته بندی محصول")]
    ProductCategory = 3,
    /// <summary>
    /// لینک منو
    /// </summary>
    [Display(Name = "لینک منو")]
    Link = 4,
    /// <summary>
    /// درباره ما
    /// </summary>
    [Display(Name = "درباره ما")]
    AboutUs = 5,
    /// <summary>
    /// اطلاعات شرکت
    /// </summary>
    [Display(Name = "اطلاعات شرکت")]
    CorporationInfo = 6,
    /// <summary>
    /// اعضای تیم
    /// </summary>
    [Display(Name = "اعضای تیم")]
    TeamMember = 7,
    /// <summary>
    /// شواهد
    /// </summary>
    [Display(Name = "شواهد")]
    Testimonial = 8,
    /// <summary>
    /// بلوک بنر
    /// </summary>
    [Display(Name = "بلوک بنر")]
    BlockBanner = 9,
    /// <summary>
    /// اسلایدر
    /// </summary>
    [Display(Name = "اسلایدر")]
    Carousel = 10,
    /// <summary>
    /// لینک شبکه های اجتماعی
    /// </summary>
    [Display(Name = "لینک شبکه های اجتماعی")]
    SocialLink = 11,
    /// <summary>
    /// تصویر محصول
    /// </summary>
    [Display(Name = "تصویر محصول")]
    ProductImage = 12,
    /// <summary>
    /// مشتری
    /// </summary>
    [Display(Name = "مشتری")]
    Customer = 13,
    /// <summary>
    /// کاربر
    /// </summary>
    [Display(Name = "کاربر")]
    User = 14,
    /// <summary>
    /// مستند سفارش بازگشت
    /// </summary>
    [Display(Name = "مستند سفارش بازگشت")]
    ReturnOrderDocument = 15,
}
