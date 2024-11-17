using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImageUrlToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Books");
        }
    }
}
