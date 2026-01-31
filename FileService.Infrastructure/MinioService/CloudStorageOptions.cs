namespace FileService.Infrastructure.MinioService;

public class CloudStorageOptions
{
    public const string SectionName = "CloudStorage";

    public string Endpoint { get; set; } = string.Empty;
    public int? Port { get; set; }
    public bool UseSsl { get; set; } = true;

    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string? SessionToken { get; set; }

    public string BucketName { get; set; } = string.Empty;
    public string? Region { get; set; }
}
