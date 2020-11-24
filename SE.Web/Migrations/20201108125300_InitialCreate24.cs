using Microsoft.EntityFrameworkCore.Migrations;

namespace SE.Web.Migrations
{
    public partial class InitialCreate24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Education",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Blog",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Blog");
        }
    }
}
