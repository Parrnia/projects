using System.ComponentModel.DataAnnotations;

namespace Onyx.Domain.Enums;

public enum ProductStockEnum
{
    [Display(Name = "In Stock")]
    InStock = 0,
    [Display(Name = "Out Of Stock")]
    OutOfStock = 1,
    [Display(Name = "On Back Order")]
    OnBackOrder = 2
}