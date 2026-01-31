namespace DomainCommons.Models.Interfaces;

public interface IHasDeletedTime
{
    DateTime? DeletedAt { get; }
}