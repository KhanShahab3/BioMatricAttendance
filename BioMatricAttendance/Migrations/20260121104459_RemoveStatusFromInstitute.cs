using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioMatricAttendance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusFromInstitute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Institutes");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceUserId",
                table: "TimeLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DeviceUserId",
                table: "TimeLogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Institutes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
