using DomainCommons.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser, IHasCreatedTime, IHasDeletedTime, ISoftDelete
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public User(string userName, string firstName, string lastName) : base(userName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.CreatedAt = DateTime.Now;
    }
    
    public void SoftDelete()
    {
        this.IsDeleted = true;
        this.DeletedAt = DateTime.Now;
    }
}