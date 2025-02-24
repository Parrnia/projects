namespace Onyx.Application.Services.SevenSoftServices;

public class SpExchange
{
    public int? Code { get; set; }
    public Guid? UniqueId { get; set; }
    public int? WarehouseSystemCode { get; set; }
    public int? WarehouseYearSystemCode { get; set; }
    public int? DealerYearSystemCode { get; set; }
    public int? DealerTypeSystemCode { get; set; }
    public int? WarehouseTypeSystemCode { get; set; }
    public int? WarehouseYearTypeSystemCode { get; set; }
    public int? DealerSystemCode { get; set; }
    public string? DealerName { get; set; }
    public string? BranchName { get; set; }
    public int? BranchSystemCode { get; set; }
    public int? DealerPreFactorCode { get; set; }
    public int? BranchPreFactorCode { get; set; }
    public int? DealerYearPreFactorCode { get; set; }
    public int? BranchYearPreFactorCode { get; set; }
    public int? WarehousePreFactorCode { get; set; }
    public DateTime? PreFactorDate { get; set; }
    public string? StrPreFactorDate { get; set; }
    public string? StrPreFactorTime { get; set; }
    public Guid? PreFactorFiscalYear { get; set; }
    public int? DealerFactorCode { get; set; }
    public long? BranchFactorCode { get; set; }
    public int? DealerYearFactorCode { get; set; }
    public object BranchYearFactorCode { get; set; }
    public int? WarehouseFactorCode { get; set; }
    public DateTime? FactorDate { get; set; }
    public string? StrFactorDate { get; set; }
    public string? StrFactorTime { get; set; }
    public Guid? FactorFiscalYear { get; set; }
    public int? DealerCollectiveInvoiceNumber { get; set; }
    public int? BranchCollectiveInvoiceNumber { get; set; }
    public int? DealerCollectiveInvoiceNumberByYear { get; set; }
    public int? BranchCollectiveInvoiceNumberByYear { get; set; }
    public Guid? DealerId { get; set; }
    public string? DealerNo { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchNo { get; set; }
    public int? SpExchangesTypeId { get; set; }
    public string? SpExchangesTypeLocalizedName { get; set; }
    public int? InventoryTypeSpExchangesId { get; set; }
    public string? InventoryTypeSpExchangesLocalizedName { get; set; }
    public Guid? SpExchangesPriorityTypeId { get; set; }
    public string? SpExchangesPriorityType { get; set; }
    public Guid? SubscriberId { get; set; }
    public string? SubscriberName { get; set; }
    public string? SubscriberNationalCode { get; set; }
    public string? SubscriberTel { get; set; }
    public string? SubscriberMobile { get; set; }
    public string? PaymentTypeId { get; set; }
    public string? PaymentTypeLocalizedName { get; set; }
    public int? SpExchangesStateId { get; set; }
    public string? SpExchangesState { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseLocalizedName { get; set; }
    public double? TotalFee { get; set; }
    public double? Tax { get; set; }
    public double? EndFee { get; set; }
    public Guid? FiscalYearId { get; set; }
    public bool? CheckTax { get; set; }
    public bool? FinalCheck { get; set; }
    public bool? FinalConfirm { get; set; }
    public bool? Lock { get; set; }
    public bool? SpExchangesListVmCollected { get; set; }
    public int? PrintCount { get; set; }
    public DateTime? Date { get; set; }
    public string? StrDate { get; set; }
    public string? StrTime { get; set; }
    public bool? IsBackOrder { get; set; }
    public bool? IsCanceled { get; set; }
    public bool? IsCentralDealer { get; set; }
    public bool? IsCentralBranchDealer { get; set; }
    public int? ExitBranchSystemCode { get; set; }
    public int? ExitWareHouseSystemCode { get; set; }
    public int? PrintCountInvoice { get; set; }
    public bool? IsGuaranty { get; set; }
    public bool? SendDocumentsOfBuyTransactions { get; set; }
    public string? TimeFromLastConfirm { get; set; }
}

public class CostCenterPart
{
    public Guid? UniqueId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? CostCenterLocalizedName { get; set; }
    public bool? IsPayableForCustomer { get; set; }
    public bool? IsInService { get; set; }
    public bool? IsOutService { get; set; }
    public bool? IsPart { get; set; }
    public bool? IsItemType { get; set; }
    public double? TotalAmount { get; set; }
    public double? Discount { get; set; }
    public double? Tax { get; set; }
    public double? EndAmount { get; set; }
}

public class Dealer
{
    public Guid? UniqueId { get; set; }
    public int? Code { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? DealerCityLocalizedName { get; set; }
    public string? DealerSubCountryLocalizedName { get; set; }
    public string? DealerEconomicCode { get; set; }
    public string? ManagerPhone { get; set; }
    public string? ManagerFax { get; set; }
    public string? ManagerEmail { get; set; }
    public string? RepairShopPhoneNumber { get; set; }
    public string? RepairShopFax { get; set; }
    public string? RepairShopEmail { get; set; }
    public string? RepairShopAddress { get; set; }
    public string? RepairShopPostalCode { get; set; }
    public string? ExhibitionEmail { get; set; }
    public string? ExhibitionPostalCode { get; set; }
    public string? DealerGrade { get; set; }
}

public class SpExchangesParts
{
    public Guid? UniqueId { get; set; }
    public string? Type { get; set; }
    public string? ServiceCodeOrPartNo { get; set; }
    public string? ServiceOrPartName { get; set; }
    public double? Number { get; set; }
    public string? Unit { get; set; }
    public double? UnitPrice { get; set; }
    public double? TotalAmount { get; set; }
    public double? Taxes { get; set; }
    public double? EndAmount { get; set; }
}

public class SevenSoftGetInvoiceResponse
{
    public SpExchange SpExchanges { get; set; }
    public List<CostCenterPart> CostCenterPart { get; set; }
    public List<Dealer> Dealer { get; set; }
    public List<SpExchangesParts> SpExchangesParts { get; set; }
}