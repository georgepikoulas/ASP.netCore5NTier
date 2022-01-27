using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.netCore5NTier.Data.Migrations
{
    public partial class AddCatTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Category");
        }
    }
}
