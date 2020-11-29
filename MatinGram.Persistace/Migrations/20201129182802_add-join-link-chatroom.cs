using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MatinGram.Persistace.Migrations
{
    public partial class addjoinlinkchatroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JoinLinkGuid",
                table: "Chatrooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinLinkGuid",
                table: "Chatrooms");
        }
    }
}
