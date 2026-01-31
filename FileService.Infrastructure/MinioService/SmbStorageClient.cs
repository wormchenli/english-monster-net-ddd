using FileService.Domain.Enum;
using FileService.Domain.External;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Infrastructure.MinioService;

public class SmbStorageClient : IStorageClient
{
    private readonly SmbStorageOptions _options;
    private readonly IMinioClient _minio;

    public StorageProviderType StorageProvider => StorageProviderType.Backup;

    public SmbStorageClient(IOptions<SmbStorageOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _minio = BuildClient(_options);
    }

    public Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (content == null || !content.CanSeek || !content.CanRead)
        {
            throw new ArgumentNullException(nameof(content));
        }

        return SaveInternalAsync(key, content, cancellationToken);
    }

    private async Task<Uri> SaveInternalAsync(string key, Stream content, CancellationToken cancellationToken)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var putArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(key)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType("application/octet-stream");

        await _minio.PutObjectAsync(putArgs, cancellationToken);

        var ub = new UriBuilder
        {
            Scheme = _options.UseSsl ? "https" : "http",
            Host = _options.Endpoint,
            Path = $"{_options.BucketName}/{key}"
        };

        if (_options.Port.HasValue)
        {
            ub.Port = _options.Port.Value;
        }

        return ub.Uri;
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        var existsArgs = new BucketExistsArgs().WithBucket(_options.BucketName);
        bool exists = await _minio.BucketExistsAsync(existsArgs, cancellationToken);
        if (exists)
        {
            return;
        }

        var makeArgs = new MakeBucketArgs()
            .WithBucket(_options.BucketName)
            .WithLocation(_options.Region);

        await _minio.MakeBucketAsync(makeArgs, cancellationToken);
    }

    private static IMinioClient BuildClient(SmbStorageOptions options)
    {
        var minioClient = new MinioClient()
            .WithEndpoint(options.Endpoint, options.Port!.Value)
            .WithCredentials(options.AccessKey, options.SecretKey)
            // Use WithSSL() if connecting via HTTPS (recommended for production)
            // .WithSSL()
            .Build();

        return minioClient;
    }
}
