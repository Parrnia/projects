using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;
public class ReturnOrderHelper : ICreateReturnOrderHelper
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ApplicationSettings _applicationSettings;

    public ReturnOrderHelper(IApplicationDbContext context, ISevenSoftService sevenSoftService, IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
        _applicationSettings = applicationSettings.Value;
    }


    public decimal CalculateTax(decimal cost, double discountPercent)
    {
        var tax = cost * ((decimal)_applicationSettings.TaxPercent / 100) * (1 - (decimal)discountPercent / 100);
        return tax;
    }

    public decimal CalculateDiscount(decimal cost, double discountPercent)
    {
        return cost * (decimal)discountPercent / 100;
    }

    public void CheckRequestAndDatabase(CreateReturnOrderCommand command, ReturnOrder returnOrder)
    {
        if (Math.Abs(command.Quantity - returnOrder.ItemGroups.Sum(c => c.ReturnOrderItems.Sum(e => e.Quantity))) > 0)
        {
            throw new ReturnOrderException("در تعداد محصولات مغایرت وجود دارد");
        }
        if (Math.Abs(10 * command.Subtotal - returnOrder.Subtotal) > 100)
        {
            throw new ReturnOrderException("در جمع قیمت محصولات مغایرت وجود دارد");
        }
        if (Math.Abs((int)(10 * command.Total - returnOrder.Total)) > 100)
        {
            throw new ReturnOrderException("در جمع قیمت کل سفارش مغایرت وجود دارد");
        }
    }

    //public SevenSoftCommand CreateSevenSoftCommand(CreateReturnOrderCommand request, ReturnOrder order, List<SevenSoftReturnOrderProductCommand> productCommands)
    //{
    //    var sevenSoftCommand = new SevenSoftCommand();

    //    var sevenSoftReturnOrderCommand = new SevenSoftReturnOrderCommand
    //    {
    //        AddSpExchangesVmSubscriberName = order.CustomerFirstName + " " + order.CustomerLastName,
    //        AddSpExchangesVmSubscriberNationalCode = request.NationalCode,
    //        AddSpExchangesVmSubscriberTel = request.PhoneNumber,
    //        AddSpExchangesVmDescription = "شماره مرتبط در سایت: " + order.Id,
    //        AddSpExchangesVmDiscountValue = 0,
    //        AddSpExchangesVmDiscountPercent = 0,
    //        RelatedCode = order.Number
    //    };

    //    var sevenSoftReturnOrderCustomerCommand = new SevenSoftReturnOrderCustomerCommand
    //    {
    //        SubscriberFirstName = request.FirstName,
    //        SubscriberLastName = request.LastName,
    //        Mobile = request.PhoneNumber
    //    };

    //    if (request.PersonType == PersonType.Natural)
    //    {
    //        sevenSoftReturnOrderCustomerCommand.SubscriberVmBirthCityRelatedCode = 0;
    //        sevenSoftReturnOrderCustomerCommand.SubscriberVmIssueCityRelatedCode = 0;
    //        sevenSoftReturnOrderCustomerCommand.IssueDate = DateTime.Now;
    //        sevenSoftReturnOrderCustomerCommand.FatherName = "";
    //        sevenSoftReturnOrderCustomerCommand.NationalCode = request.NationalCode;
    //        sevenSoftReturnOrderCustomerCommand.IdNumber = "";
    //        sevenSoftReturnOrderCustomerCommand.Gender = 0;
    //        sevenSoftReturnOrderCustomerCommand.BirthDate = new DateTime(1753, 1, 1);
    //        sevenSoftReturnOrderCustomerCommand.HomeAddress = request.AddressDetails1 + request.AddressDetails2;
    //        sevenSoftReturnOrderCustomerCommand.HomeTel = request.PhoneNumber;
    //        sevenSoftReturnOrderCustomerCommand.IsLegalSubscriber = false;

    //    }
    //    else if (request.PersonType == PersonType.Legal)
    //    {
    //        sevenSoftReturnOrderCustomerCommand.CompanyName = request.FirstName;
    //        sevenSoftReturnOrderCustomerCommand.NationalId = request.NationalCode;
    //        sevenSoftReturnOrderCustomerCommand.NationalCode = request.NationalCode;
    //        sevenSoftReturnOrderCustomerCommand.EconomicCode = request.LastName;
    //        sevenSoftReturnOrderCustomerCommand.ManagerName = "";
    //        sevenSoftReturnOrderCustomerCommand.OfficeAddress = request.AddressDetails1 + request.AddressDetails2;
    //        sevenSoftReturnOrderCustomerCommand.OfficeTel = request.PhoneNumber;
    //        sevenSoftReturnOrderCustomerCommand.IsLegalSubscriber = true;
    //        sevenSoftReturnOrderCustomerCommand.IssueDate = DateTime.Now;
    //        sevenSoftReturnOrderCustomerCommand.BirthDate = new DateTime(1753, 1, 1);
    //    }

    //    sevenSoftReturnOrderCommand.AddSpExchangesVmSpExchangesParts = productCommands;

    //    sevenSoftCommand.SubscriberComplete = sevenSoftReturnOrderCustomerCommand;
    //    sevenSoftCommand.AddSpExchanges = sevenSoftReturnOrderCommand;

    //    return sevenSoftCommand;
    //}
}
