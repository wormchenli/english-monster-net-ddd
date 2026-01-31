using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileService.Domain.Enum;

namespace FileService.Domain.External;

public interface IStorageClient
{
    StorageProviderType StorageProvider { get; }

    Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default);
}