using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SE.Web.Migrations
{
    public partial class NewTableCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Education",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EducationCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Education_CategoryId",
                table: "Education",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_EducationCategory_CategoryId",
                table: "Education",
                column: "CategoryId",
                principalTable: "EducationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_EducationCategory_CategoryId",
                table: "Education");

            migrationBuilder.DropTable(
                name: "EducationCategory");

            migrationBuilder.DropIndex(
                name: "IX_Education_CategoryId",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Education");
        }
    }
}
