using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_fs_uploaded-items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FileSha256Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FileBackupPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileAccessPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_fs_uploaded-items", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_fs_uploaded-items_FileSha256Hash_FileSizeInBytes",
                table: "t_fs_uploaded-items",
                columns: new[] { "FileSha256Hash", "FileSizeInBytes" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_fs_uploaded-items");
        }
    }
}
