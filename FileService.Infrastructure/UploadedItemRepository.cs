using FileService.Domain.Entity;
using FileService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

public class UploadedItemRepository : IUploadedItemRepository
{
    private readonly FileServiceDbContext _dbContext;

    public UploadedItemRepository(FileServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UploadedItem?> FindFileAsync(string fileSha256Hash, long fileSizeInBytes)
    {
        return _dbContext.UploadedItems.FirstOrDefaultAsync(
            i=>i.FileSha256Hash == fileSha256Hash 
            && 
            i.FileSizeInBytes == fileSizeInBytes);
    }

    public async Task AddAsync(UploadedItem item)
    {
        var existing = await FindFileAsync(item.FileSha256Hash, item.FileSizeInBytes);
        if (existing != null)
        {
            return;
        }

        await _dbContext.UploadedItems.AddAsync(item);
        // await _dbContext.SaveChangesAsync();
    }
}
