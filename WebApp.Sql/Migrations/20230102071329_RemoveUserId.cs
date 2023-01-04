using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Sql.Migrations
{
    public partial class RemoveUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUserLogins_UserLoginProvider_UserProviderKey",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserLoginProvider_UserProviderKey",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UserLoginProvider",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UserProviderKey",
                table: "Likes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Likes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UserLoginProvider",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserProviderKey",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserLoginProvider_UserProviderKey",
                table: "Likes",
                columns: new[] { "UserLoginProvider", "UserProviderKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUserLogins_UserLoginProvider_UserProviderKey",
                table: "Likes",
                columns: new[] { "UserLoginProvider", "UserProviderKey" },
                principalTable: "AspNetUserLogins",
                principalColumns: new[] { "LoginProvider", "ProviderKey" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
