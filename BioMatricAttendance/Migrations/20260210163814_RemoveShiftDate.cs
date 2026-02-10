using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioMatricAttendance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShiftDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftDate",
                table: "CandidateShifts");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateShifts_ShiftId",
                table: "CandidateShifts",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateShifts_ShiftTypes_ShiftId",
                table: "CandidateShifts",
                column: "ShiftId",
                principalTable: "ShiftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateShifts_ShiftTypes_ShiftId",
                table: "CandidateShifts");

            migrationBuilder.DropIndex(
                name: "IX_CandidateShifts_ShiftId",
                table: "CandidateShifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShiftDate",
                table: "CandidateShifts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
