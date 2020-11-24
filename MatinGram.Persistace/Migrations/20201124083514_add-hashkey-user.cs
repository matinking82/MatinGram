using Microsoft.EntityFrameworkCore.Migrations;

namespace MatinGram.Persistace.Migrations
{
    public partial class addhashkeyuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashKey",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashKey",
                table: "Users");
        }
    }
}
