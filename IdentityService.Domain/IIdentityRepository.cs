using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain;

public interface IIdentityRepository
{
    Task<User?> FindByIdAsync(string userId);
    
    Task<User?> FindByUserNameAsync(string userName);
    
    Task<User?> FindByPhoneNumberAsync(string phoneNumber);
    
    // Task<User?> FindByEmailAsync(string email);
    
    Task<IdentityResult> CreateUserAsync(User user, string password);
    
    
}