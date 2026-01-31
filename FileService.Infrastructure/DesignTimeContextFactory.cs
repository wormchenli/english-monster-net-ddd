using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FileService.Infrastructure;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<FileServiceDbContext>
{
    public FileServiceDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("FileServiceDb");

        DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
        builder.UseSqlServer(connectionString);

        return new FileServiceDbContext(builder.Options);
    }
}
