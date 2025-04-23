using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BNS2025.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class testit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add new int identity column
            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 2. Drop foreign keys referencing Accounts.Id (do this manually if you have any)

            // 3. Drop primary key constraint (EF will usually name it PK_Accounts)
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            // 4. Drop the old string Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            // 5. Rename NewId to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Accounts",
                newName: "Id");

            // 6. Add primary key back on the new Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            // 7. Re-add any foreign keys manually (if applicable)
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the steps

            // 1. Add old string Id column back
            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // 2. Drop primary key on int column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            // 3. Drop the current int Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            // 4. Rename OldId to Id
            migrationBuilder.RenameColumn(
                name: "OldId",
                table: "Accounts",
                newName: "Id");

            // 5. Add primary key back on string Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");
        }
    }
}