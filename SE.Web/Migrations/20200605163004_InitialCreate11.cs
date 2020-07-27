using Microsoft.EntityFrameworkCore.Migrations;

namespace SE.Web.Migrations
{
    public partial class InitialCreate11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookAccountUrl",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramAccountUrl",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapCode",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterAccountUrl",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeAccountUrl",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoOne",
                table: "Education",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoTwo",
                table: "Education",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookAccountUrl",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "InstagramAccountUrl",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "MapCode",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "TwitterAccountUrl",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "YoutubeAccountUrl",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoOne",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoTwo",
                table: "Education");
        }
    }
}
