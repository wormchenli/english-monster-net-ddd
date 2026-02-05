using CommonUtils;
using DomainCommons.Models.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure;

public class IdentityDbContext : IdentityDbContext<User, Role, string>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        
        // apply global query filter for soft delete
        builder.EnableSoftDeleteFilter<ISoftDelete>(propertyName:"IsDeleted");
    }
}