using Microsoft.Extensions.DependencyInjection;

namespace CommonInitializer;

public interface IModuleInitializer
{
    void Register(IServiceCollection services);
}