using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuongNDH2_PersonalDiaryAPI.Migrations
{
    public partial class AddIsPublish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPublish",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPublish",
                table: "Posts");
        }
    }
}
