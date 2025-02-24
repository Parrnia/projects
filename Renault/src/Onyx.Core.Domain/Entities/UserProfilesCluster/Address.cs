using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Domain.Entities.UserProfilesCluster;
public class Address : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// شرکت
    /// </summary>
    public string? Company { get; set; }
    /// <summary>
    /// کشور
    /// </summary>
    public Country Country { get; set; } = null!;
    public int CountryId { get; set; }
    /// <summary>
    /// جزئیات آدرس
    /// </summary>
    public string AddressDetails1 { get; set; } = null!;
    /// <summary>
    /// جزئیات بیشتر آدرس
    /// </summary>
    public string? AddressDetails2 { get; set; }
    /// <summary>
    /// شهر
    /// </summary>
    public string City { get; set; } = null!;
    /// <summary>
    /// منطقه
    /// </summary>
    public string State { get; set; } = null!;
    /// <summary>
    /// کد پستی
    /// </summary>
    public string Postcode { get; set; } = null!;
    /// <summary>
    /// پیش فرض است؟
    /// </summary>
    public bool Default { get; set; }
    /// <summary>
    /// مشتری مرتبط
    /// </summary>
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
}