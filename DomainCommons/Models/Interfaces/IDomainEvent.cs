using MediatR;

namespace DomainCommons.Models.Interfaces;

public interface IDomainEvent
{
    IEnumerable<INotification> GetDomainEvents();
    
    void AddDomainEvent(INotification @event);
    
    void AddDomainEventIfAbsent(INotification @event);
    
    void ClearDomainEvents();
}