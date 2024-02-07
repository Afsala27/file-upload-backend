using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUploadApplication.Migrations
{
    /// <inheritdoc />
    public partial class initialpush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDatas",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDatas", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ImgDatas",
                columns: table => new
                {
                    ImgDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgDatas", x => x.ImgDataId);
                    table.ForeignKey(
                        name: "FK_ImgDatas_UserDatas_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDatas",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImgDatas_UserId",
                table: "ImgDatas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImgDatas");

            migrationBuilder.DropTable(
                name: "UserDatas");
        }
    }
}
