using System.ComponentModel.DataAnnotations.Schema;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CustomerSupportCluster;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Interfaces;

namespace Onyx.Domain.Entities.UserProfilesCluster;
public class Customer : IChangeDateAware
{
    public Customer()
    {
        Created = DateTime.Now;
    }
    /// <summary>
    /// Base Properties
    /// </summary>
    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }

    public Guid Id { get; set; }
    /// <summary>
    /// آواتار
    /// </summary>
    public Guid? Avatar { get; set; }
    /// <summary>
    /// اعتبار مشتری
    /// </summary>
    public List<Credit> Credits { get; set; } = new List<Credit>();
    /// <summary>
    /// سقف اعتبار مشتری
    /// </summary>
    public List<MaxCredit> MaxCredits { get; set; } = new List<MaxCredit>();
    /// <summary>
    /// آدرس ها
    /// </summary>
    public List<Address> Addresses { get; set; } = new List<Address>();
    /// <summary>
    /// سفارش ها
    /// </summary>
    public List<Order> Orders { get; set; } = new List<Order>();
    /// <summary>
    /// نظرات
    /// </summary>
    public List<Review> Reviews { get; set; } = new List<Review>();
    /// <summary>
    /// نوع خودروهای مشتری
    /// </summary>
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    /// <summary>
    /// تیکت های مشتری
    /// </summary>
    public List<CustomerTicket> CustomerTickets { get; set; } = new List<CustomerTicket>();
    /// <summary>
    /// نظرات
    /// </summary>
    public List<Comment> Comments { get; set; } = new List<Comment>();
    /// <summary>
    /// نظرات ویجت
    /// </summary>
    public List<WidgetComment> WidgetComments { get; set; } = new List<WidgetComment>();

    public decimal GetMaxCredit()
    {
        return MaxCredits.Count > 0 ? MaxCredits.OrderBy(c => c.Date).Last().Value : 0;
    }

    public bool SetMaxCredit(decimal maxCredit, Guid modifierUserId, string modifierUserName)
    {
        if (maxCredit < 0)
        {
            return false;
        }

        var currentCredit = Credits.OrderBy(c => c.Date).LastOrDefault();
        if (currentCredit != null && maxCredit < currentCredit.Value)
        {
            Credits.Add(new Credit()
            {
                Date = DateTime.Now,
                Value = maxCredit,
                ModifierUserId = modifierUserId,
                ModifierUserName = modifierUserName
            });
        }

        MaxCredits.Add(new MaxCredit()
        {
            Date = DateTime.Now,
            Value = maxCredit,
            ModifierUserId = modifierUserId,
            ModifierUserName = modifierUserName
        });
        return true;
    }

    public decimal GetCredit()
    {
        return Credits.Count > 0 ? Credits.OrderBy(c => c.Date).Last().Value : 0;
    }

    public Credit GetCurrentCredit()
    {
        return Credits.Count > 0 ? Credits.OrderBy(c => c.Date).Last() : throw new DomainException("مشکلی در اعتبار مشتری رخ داده است");
    }


    public bool SetCredit(decimal credit, Guid modifierUserId, string modifierUserName)
    {
        if (credit < 0 || GetMaxCredit() < credit)
        {
            return false;
        }
        Credits.Add(new Credit()
        {
            Date = DateTime.Now,
            Value = credit,
            ModifierUserId = modifierUserId,
            ModifierUserName = modifierUserName
        });
        return true;
    }

    public bool UseCredit(decimal orderTotal, Guid modifierUserId, string modifierUserName, string orderToken)
    {
        if (GetCredit() - orderTotal < 0)
        {
            return false;
        }

        if (Credits.Where(c => c.OrderToken == orderToken).ToList().Count == 0)
        {

            Credits.Add(new Credit()
            {
                Date = DateTime.Now,
                Value = GetCredit() - orderTotal,
                ModifierUserId = modifierUserId,
                ModifierUserName = modifierUserName,
                OrderToken = orderToken
            });
        }
        return true;
    }

    public decimal UsePortionCredit(decimal orderTotal, Guid modifierUserId, string modifierUserName, string orderToken)
    {
        if (GetCredit() - orderTotal > 0)
        {
            return 0;
        }
        var remainedCost = orderTotal - GetCredit();

        if (Credits.Where(c => c.OrderToken == orderToken).ToList().Count == 0)
        {
            Credits.Add(new Credit()
            {
                Date = DateTime.Now,
                Value = 0,
                ModifierUserId = modifierUserId,
                ModifierUserName = modifierUserName,
                OrderToken = orderToken
            });
        }

        return remainedCost;
    }


    public bool AddCredit(decimal credit, Guid modifierUserId, string modifierUserName, string orderToken)
    {
        if (credit < 0)
        {
            return false;
        }

        Credits.Add(new Credit()
        {
            Date = DateTime.Now,
            Value = GetCredit() + credit,
            ModifierUserId = modifierUserId,
            ModifierUserName = modifierUserName,
            OrderToken = orderToken + '-'
        });


        return true;
    }

    public bool AddCreditWithoutLog(decimal creditValue)
    {
        if (creditValue < 0)
        {
            return false;
        }

        var credit = GetCurrentCredit();
        credit.Value = creditValue;
        return true;
    }
}