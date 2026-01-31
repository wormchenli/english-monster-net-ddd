using System.Threading;
using System.Threading.Tasks;
using FileService.Domain.Entity;

namespace FileService.Domain.Repository;

public interface IUploadedItemRepository
{
    // find if the file is existed by looking up the sha256 hash and file size passed in
    // using sha256 is pretty much enough as the probability of getting 2 identical sha256 value is quite low,
    // but just in case, so add file size as well
    Task<UploadedItem?> FindFileAsync(string fileSha256Hash, long fileSizeInBytes);

    Task AddAsync(UploadedItem item);
}
