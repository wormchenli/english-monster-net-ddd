using CommonInitializer;
using FileService.Domain.DomainService;
using FileService.Domain.External;
using FileService.Domain.Repository;
using FileService.Infrastructure.MinioService;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

public class ModuleInitialize : IModuleInitializer
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<FsDomainService>();
        services.AddScoped<IStorageClient, CloudStorageClient>();
        services.AddScoped<IStorageClient, SmbStorageClient>();
        services.AddScoped<IUploadedItemRepository, UploadedItemRepository>();
        services.AddScoped<FsDomainService>();
    }
}