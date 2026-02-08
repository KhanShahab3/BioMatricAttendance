using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioMatricAttendance.Migrations
{
    /// <inheritdoc />
    public partial class addRegionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "AppUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "AppUsers");
        }
    }
}
