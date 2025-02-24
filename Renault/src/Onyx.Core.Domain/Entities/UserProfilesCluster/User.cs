using System.ComponentModel.DataAnnotations.Schema;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Interfaces;

namespace Onyx.Domain.Entities.UserProfilesCluster;
public class User : IChangeDateAware
{
    public User()
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
    /// پست ها
    /// </summary>
    public List<Post> Posts { get; set; } = new List<Post>();
}