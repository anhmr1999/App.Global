using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Global.Migrations
{
    public partial class addtitletoemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AppService_SendMails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "AppService_SendMails");
        }
    }
}
