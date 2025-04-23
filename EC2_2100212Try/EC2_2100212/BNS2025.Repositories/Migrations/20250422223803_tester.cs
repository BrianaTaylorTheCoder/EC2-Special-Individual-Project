using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BNS2025.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class tester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                 name: "NewId",
                 table: "Accounts",
                 type: "nvarchar(450)",
                 nullable: false,
                 defaultValue: "");

            // Step 2: Copy existing int Id into the new string column
            migrationBuilder.Sql("UPDATE Accounts SET NewId = CAST(Id AS nvarchar(450))");

            // Step 3: Drop the existing primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            // Step 4: Drop the old int Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            // Step 5: Rename the NewId column to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Accounts",
                newName: "Id");

            // Step 6: Add a new primary key on the new string Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a new int column to reverse the Id change
            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Step 2: Copy string Id back into OldId (as int)
            migrationBuilder.Sql("UPDATE Accounts SET OldId = CAST(Id AS int)");

            // Step 3: Drop the current primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            // Step 4: Drop the string Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            // Step 5: Rename OldId back to Id
            migrationBuilder.RenameColumn(
                name: "OldId",
                table: "Accounts",
                newName: "Id");

            // Step 6: Recreate primary key on reverted Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");
        }
    }
}