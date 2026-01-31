
namespace FileService.Infrastructure.MinioService;

public class SmbStorageOptions
{
    public const string SectionName = "SmbStorage";

    public string Endpoint { get; set; } = string.Empty;
    public int? Port { get; set; }
    public bool UseSsl { get; set; }

    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string? SessionToken { get; set; }

    public string BucketName { get; set; } = string.Empty;
    public string? Region { get; set; }
}
