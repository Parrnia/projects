
namespace Onyx.Application.Services.SevenSoftServices;

public class CheckProductPriceResult
{
    public List<ProductPrice>? ProductPrices { get; set; }
    public bool IsValid { get; set; }
}