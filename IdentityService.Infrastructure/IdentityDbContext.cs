using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityService.Infrastructure;

public class IdentityDbContext:IdentityDbContext<User, Role, string>
{
}