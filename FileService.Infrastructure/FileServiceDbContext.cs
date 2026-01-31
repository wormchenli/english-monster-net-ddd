using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

public class FileServiceDbContext : DbContext
{
    public DbSet<UploadedItem> UploadedItems { get; private set; }
    
    public FileServiceDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}