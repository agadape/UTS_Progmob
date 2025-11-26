using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTalk.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImageUrlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Courses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
       
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Courses");
        }
    }
}
