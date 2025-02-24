namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftOrderCustomerCommand
{
    public SevenSoftOrderCustomerCommand()
    {

    }
    /// <summary>
    /// نام کمپانی
    /// </summary>
    public string? CompanyName { get; set; } = null!;
    /// <summary>
    /// شناسه ملی
    /// </summary>
    public string? NationalId { get; set; } = null!;
    /// <summary>
    /// کد اقتصادی
    /// </summary>
    public string? EconomicCode { get; set; } = null!;
    /// <summary>
    /// نام مدیر
    /// </summary>
    public string? ManagerName { get; set; } = null!;
    /// <summary>
    /// آدرس اداره
    /// </summary>
    public string? OfficeAddress { get; set; } = null!;
    /// <summary>
    /// شماره تماس اداره
    /// </summary>
    public string? OfficeTel { get; set; } = null!;



    
    /// <summary>
    /// محل تولد مشتری
    /// </summary>
    public int SubscriberVmBirthCityRelatedCode { get; set; }
    /// <summary>
    /// محل صدور شناسنامه
    /// </summary>
    public int SubscriberVmIssueCityRelatedCode { get; set; }
    /// <summary>
    /// تاریخ صدور شناسنامه
    /// </summary>
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// نام پدر مشتری
    /// </summary>
    public string? FatherName { get; set; } = null!;
    /// <summary>
    /// کد ملی مشتری
    /// </summary>
    public string? NationalCode { get; set; } = null!;
    /// <summary>
    /// شماره شناسنامه
    /// </summary>
    public string? IdNumber { get; set; } = null!;
    /// <summary>
    /// جنسیت
    /// </summary>
    public int Gender { get; set; }
    /// <summary>
    /// تاریخ تولد
    /// </summary>
    public DateTime BirthDate { get; set; }
    /// <summary>
    /// آدرس منزل
    /// </summary>
    public string? HomeAddress { get; set; } = null!;
    /// <summary>
    /// شماره تماس خانه
    /// </summary>
    public string? HomeTel { get; set; } = null!;
    



    /// <summary>
    /// نام مشتری
    /// </summary>
    public string SubscriberFirstName { get; set; } = null!;
    /// <summary>
    /// نام خانوادگی مشتری
    /// </summary>
    public string SubscriberLastName { get; set; } = null!;
    /// <summary>
    /// شماره همراه
    /// </summary>
    public string Mobile { get; set; } = null!;




    /// <summary>
    /// حقیقی یا حقوقی
    /// </summary>
    public bool IsLegalSubscriber { get; set; }

}
