using System;
using DomainCommons.Models;
using DomainCommons.Models.Interfaces;

namespace  FileService.Domain.Entity;

public record UploadedItem(
    long FileSizeInBytes,
    string FileName,
    string FileSha256Hash,
    Uri FileBackupPath,
    Uri FileAccessPath)
    : BaseEntity, IHasCreatedTime
{
    public DateTime CreatedAt { get; private set; } = DateTime.Now;

    public long FileSizeInBytes { get; private set; } = FileSizeInBytes;

    public string FileName { get; private set; } = FileName;

    public string FileSha256Hash { get; private set; } = FileSha256Hash;

    public Uri FileBackupPath { get; private set; } = FileBackupPath;

    public Uri FileAccessPath { get; private set; } = FileAccessPath;
    
    // use static factory method may make the semantics more clear, i.e., easy to think about
    public static UploadedItem CreateItem(Guid id, long fileSizeInBytes, string fileName, string fileSha256Hash, Uri fileBackupPath, Uri fileAccessPath)
    {
        return new UploadedItem(fileSizeInBytes, fileName, fileSha256Hash, fileBackupPath, fileAccessPath)
        {
            Id = id
        };
    }
}
