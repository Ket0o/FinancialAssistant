using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Accounts");
        }
    }
}
