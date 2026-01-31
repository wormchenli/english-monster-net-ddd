using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonUtils;
using FileService.Domain.Entity;
using FileService.Domain.Enum;
using FileService.Domain.External;
using FileService.Domain.Repository;

namespace FileService.Domain.DomainService;

public class FsDomainService
{
    // dependencies injection
    private readonly IUploadedItemRepository _repository;

    private readonly IStorageClient _remoteClient;
    
    private readonly IStorageClient _backupClient;

    public FsDomainService(IUploadedItemRepository repository, 
        IEnumerable<IStorageClient> clients) 
    {
        _repository = repository;

        var clientList = clients.ToList();
        // IStorageClient has a SET ONLY property 'StorageProvider'
        // meaning that it must be specified when initiating the instance
        // by 'c.StorageProvider == StorageProviderType.Public'
        _remoteClient = clientList.Single(c => c.StorageProvider == StorageProviderType.Public);
        _backupClient = clientList.Single(c=> c.StorageProvider == StorageProviderType.Backup);
    }

    public async Task<UploadedItem?> UpLoadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        string hash = HashHelper.GenerateSha256Hash(fileStream);
        long size = fileStream.Length;
        DateTime today = DateTime.Today;

        string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}_{size}_{fileName}";
        
        var uploadedFile = await _repository.FindFileAsync(fileSha256Hash: hash, fileSizeInBytes: size);
        
        // if the file is existed, then return it
        // if the file is not existed, then upload it
        if (uploadedFile != null)
        {
            return uploadedFile;
        }

        fileStream.Position = 0;
        Uri accessUri = await _remoteClient.SaveAsync(key, fileStream, cancellationToken);
        fileStream.Position = 0;
        Uri backupUri = await _backupClient.SaveAsync(key, fileStream, cancellationToken);
        fileStream.Position = 0;

        Guid id = Guid.NewGuid();

        return UploadedItem.CreateItem(id, size, fileName, hash, backupUri, accessUri);
    }
}