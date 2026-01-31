using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.EntityConfigs;

public class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>
{
    public void Configure(EntityTypeBuilder<UploadedItem> builder)
    {
        builder.ToTable("t_fs_uploaded-items");

        // opt out clustered index for guid type primary key
        // as everytime with a record inserted, it will cause db to reorganize the whole table data pages
        // and guid is not sequential and helpful in this case
        builder.HasKey(x => x.Id).IsClustered(false);
        builder.Property(x => x.FileName).HasMaxLength(1024);
        builder.Property(x => x.FileSha256Hash).HasMaxLength(64);
        
        // we may use the combination of  the 2 columns as index 
        // cuz we will look up the uploaded item by using these 2 columns
        builder.HasIndex(x => new { x.FileSha256Hash, x.FileSizeInBytes });
    }
}