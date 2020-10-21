using Microsoft.EntityFrameworkCore.Migrations;

namespace SE.Web.Migrations
{
    public partial class InitialCreate22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_AttributeCategory_AttributeCategoryId",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeCategory_AttributeCategory_AttributeCategoryId",
                table: "CategoryAttributeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeCategory_Category_CategoryId",
                table: "CategoryAttributeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_Category_CategoryId",
                table: "Education");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_AttributeCategory_AttributeCategoryId",
                table: "Attribute",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeCategory_AttributeCategory_AttributeCategoryId",
                table: "CategoryAttributeCategory",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeCategory_Category_CategoryId",
                table: "CategoryAttributeCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Category_CategoryId",
                table: "Education",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_AttributeCategory_AttributeCategoryId",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeCategory_AttributeCategory_AttributeCategoryId",
                table: "CategoryAttributeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeCategory_Category_CategoryId",
                table: "CategoryAttributeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_Category_CategoryId",
                table: "Education");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_AttributeCategory_AttributeCategoryId",
                table: "Attribute",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeCategory_AttributeCategory_AttributeCategoryId",
                table: "CategoryAttributeCategory",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeCategory_Category_CategoryId",
                table: "CategoryAttributeCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Category_CategoryId",
                table: "Education",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
