using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUploadApplication.Migrations
{
    /// <inheritdoc />
    public partial class addingdriveid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriveFileId",
                table: "ImgDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriveFileId",
                table: "ImgDatas");
        }
    }
}
