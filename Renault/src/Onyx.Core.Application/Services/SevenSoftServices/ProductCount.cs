
namespace Onyx.Application.Services.SevenSoftServices;

public class ProductCount
{
    public Guid PartId { get; set; }
    public double Number { get; set; }
    public double OrderQuantity { get; set; }
    public int TransactionTypeId { get; set; }
}