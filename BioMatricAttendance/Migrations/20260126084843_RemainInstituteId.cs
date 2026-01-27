using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioMatricAttendance.Migrations
{
    /// <inheritdoc />
    public partial class RemainInstituteId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Instituteid",
                table: "AppUsers",
                newName: "InstituteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "AppUsers",
                newName: "Instituteid");
        }
    }
}
