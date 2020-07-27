using Microsoft.EntityFrameworkCore.Migrations;

namespace SE.Web.Migrations
{
    public partial class InitialCreate20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeoUrl",
                table: "District",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoUrl",
                table: "District");
        }
    }
}
