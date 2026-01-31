using FileService.Domain.Enum;
using FileService.Domain.External;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Infrastructure.MinioService;

public class CloudStorageClient : IStorageClient
{
    private readonly CloudStorageOptions _options;
    private readonly IMinioClient _minio;
    public StorageProviderType StorageProvider => StorageProviderType.Public;

    public CloudStorageClient(IOptions<CloudStorageOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        // only inject options, manually build Minio client
        _minio = BuildClient(_options);
    }

    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        // 1. verify parameters
        if(string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        if(content == null || !content.CanSeek || !content.CanRead) throw  new  ArgumentNullException (nameof(content));
        
        // 2. ensure bucket exists
        await EnsureBucketExistsAsync(cancellationToken);
        
        // 3. construct put args and put object
        var putArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(key)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType("application/octet-stream");
        
        await _minio.PutObjectAsync(putArgs, cancellationToken);
        
        // 4. construct and return the URI
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

    private static IMinioClient BuildClient(CloudStorageOptions options)
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
