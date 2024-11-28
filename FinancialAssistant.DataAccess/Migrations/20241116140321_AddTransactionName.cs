using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Transactions");
        }
    }
}
