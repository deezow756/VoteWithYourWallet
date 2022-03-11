using Microsoft.EntityFrameworkCore.Migrations;

namespace VoteWithYourWallet.Migrations
{
    public partial class added_name_to_signatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Signature",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Signature");
        }
    }
}
