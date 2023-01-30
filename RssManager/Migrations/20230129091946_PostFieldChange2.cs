using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssManager.Migrations
{
    /// <inheritdoc />
    public partial class PostFieldChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "Summary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Posts",
                newName: "Content");
        }
    }
}
