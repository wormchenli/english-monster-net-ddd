using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace FileService.Api.Validations;

public sealed class FileSizeAttribute : ValidationAttribute
{
    private readonly string _configKey;
    
    public FileSizeAttribute(string configKey)
    {
        _configKey = configKey;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not IFormFile file)
        {
            return new ValidationResult("Invalid file.");
        }

        // get configuration from DI container
        var config = validationContext.GetService(typeof(IConfiguration)) as IConfiguration;

        if (config is null) return new ValidationResult("Invalid configuration. Check `appsettings`.");
        
        var maxBytes = config.GetValue<long>(_configKey);
        if (maxBytes <= 0) return new ValidationResult("Invalid file size.");
        
        return file.Length <= maxBytes
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? $"File size must be less than or equal to {maxBytes} bytes.");
        
        // var configuration = (IConfiguration?)validationContext.GetService(typeof(IConfiguration));
        // if (configuration is null)
        // {
        //     return new ValidationResult("Configuration is unavailable for file size validation. check appsettings");
        // }
        //
        // var maxBytes = configuration.GetValue<long>(ConfigKey);
        // if (maxBytes <= 0)
        // {
        //     return new ValidationResult($"Invalid file size limit configured at '{ConfigKey}'.");
        // }
        //
        // return file.Length <= maxBytes
        //     ? ValidationResult.Success
        //     : new ValidationResult(ErrorMessage ?? $"File size must be less than or equal to {maxBytes} bytes.");
    }
}
