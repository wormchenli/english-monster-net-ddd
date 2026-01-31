using DomainCommons.Models.Interfaces;

namespace DomainCommons.Models;

public record AggregateRoot : BaseEntity, IAggregateRoot, ISoftDelete, IHasCreatedTime, IHasDeletedTime, IHasUpdatedTime
{
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    
    public DateTime? UpdatedAt { get; private set; }
    
    public DateTime? DeletedAt { get; private set; }
    
    public bool IsDeleted { get; private set; }
    
    public virtual void SoftDelete()
    {
        this.IsDeleted = true;
        this.DeletedAt = DateTime.Now;
    }

    public void NotifyUpdated()
    {
        this.UpdatedAt = DateTime.Now;
    }
    
}