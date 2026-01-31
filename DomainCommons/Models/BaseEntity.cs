using System.ComponentModel.DataAnnotations.Schema;
using DomainCommons.Models.Interfaces;
using MediatR;

namespace DomainCommons.Models;

public record BaseEntity : IEntity, IDomainEvent
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [NotMapped]
    private readonly List<INotification> _domainEvents = new();
    
    public IEnumerable<INotification> GetDomainEvents()
    {
        return _domainEvents;
    }

    public void AddDomainEvent(INotification @event)
    {
        _domainEvents.Add(@event);
    }

    public void AddDomainEventIfAbsent(INotification @event)
    {
        if (!_domainEvents.Contains(@event))
        {
            _domainEvents.Add(@event);
        }
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}