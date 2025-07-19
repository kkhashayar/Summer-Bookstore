using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summer_Bookstore_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Username_In_AuditEntry_Removed_UserId_And_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditEntries_Users_UserId",
                table: "AuditEntries");

            migrationBuilder.DropIndex(
                name: "IX_AuditEntries_UserId",
                table: "AuditEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditEntries");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "AuditEntries");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AuditEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_UserId",
                table: "AuditEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditEntries_Users_UserId",
                table: "AuditEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
