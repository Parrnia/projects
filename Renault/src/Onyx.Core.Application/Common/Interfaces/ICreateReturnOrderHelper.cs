using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Common.Interfaces;
public interface ICreateReturnOrderHelper
{
    public decimal CalculateTax(decimal cost, double discountPercent);

    public decimal CalculateDiscount(decimal cost, double discountPercent);

    public void CheckRequestAndDatabase(CreateReturnOrderCommand command, ReturnOrder returnOrder);

    //public SevenSoftCommand CreateSevenSoftCommand(CreateReturnOrderCommand request, ReturnOrder order,
    //    List<SevenSoftReturnOrderProductCommand> productCommands);
}
