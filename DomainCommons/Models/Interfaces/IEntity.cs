namespace DomainCommons.Models.Interfaces;

public interface IEntity
{
    // guid is just for entity-wise connection, not for physical primary key in database
    // since using guid as physical primary key in mysql has very poor performance
    public Guid Id { get; }
    
}