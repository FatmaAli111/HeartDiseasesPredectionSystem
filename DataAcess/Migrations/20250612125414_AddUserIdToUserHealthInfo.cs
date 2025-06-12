using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToUserHealthInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserHealthInfos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserHealthInfos_UserId",
                table: "UserHealthInfos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHealthInfos_AspNetUsers_UserId",
                table: "UserHealthInfos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHealthInfos_AspNetUsers_UserId",
                table: "UserHealthInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserHealthInfos_UserId",
                table: "UserHealthInfos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserHealthInfos");
        }
    }
}
