using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestFormNumber",
                table: "LeaveRequests",
                type: "text",
                nullable: false,
                defaultValueSql: "'LRF-000' || nextval('\"TempNumber\"')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "'\"LRF-000\"' || nextval('\"TempNumber\"')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestFormNumber",
                table: "LeaveRequests",
                type: "text",
                nullable: false,
                defaultValueSql: "'\"LRF-000\"' || nextval('\"TempNumber\"')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "'LRF-000' || nextval('\"TempNumber\"')");
        }
    }
}
