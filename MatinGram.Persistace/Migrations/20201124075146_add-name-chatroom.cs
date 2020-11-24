using Microsoft.EntityFrameworkCore.Migrations;

namespace MatinGram.Persistace.Migrations
{
    public partial class addnamechatroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Chatrooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Chatrooms");
        }
    }
}
