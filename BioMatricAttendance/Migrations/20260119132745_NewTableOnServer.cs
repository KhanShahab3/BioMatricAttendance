using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BioMatricAttendance.Migrations
{
    /// <inheritdoc />
    public partial class NewTableOnServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculities_Institutes_InstituteId",
                table: "Faculities");

            migrationBuilder.DropForeignKey(
                name: "FK_FaculityAttendances_Faculities_FaculityId",
                table: "FaculityAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_FaculityAttendances_Institutes_InstituteId",
                table: "FaculityAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Courses_CourseId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Institutes_InstituteId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Institutes_InstituteId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FaculityAttendances",
                table: "FaculityAttendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Faculities",
                table: "Faculities");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "BiomatricDevices");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "StudentAttendances",
                newName: "StudentAttendance");

            migrationBuilder.RenameTable(
                name: "FaculityAttendances",
                newName: "FaculityAttendance");

            migrationBuilder.RenameTable(
                name: "Faculities",
                newName: "Faculity");

            migrationBuilder.RenameColumn(
                name: "BiomerticUserId",
                table: "DeviceAttendanceLogs",
                newName: "DeviceUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_InstituteId",
                table: "Student",
                newName: "IX_Student_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CourseId",
                table: "Student",
                newName: "IX_Student_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendance",
                newName: "IX_StudentAttendance_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendances_InstituteId",
                table: "StudentAttendance",
                newName: "IX_StudentAttendance_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendances_CourseId",
                table: "StudentAttendance",
                newName: "IX_StudentAttendance_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_FaculityAttendances_InstituteId",
                table: "FaculityAttendance",
                newName: "IX_FaculityAttendance_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_FaculityAttendances_FaculityId",
                table: "FaculityAttendance",
                newName: "IX_FaculityAttendance_FaculityId");

            migrationBuilder.RenameIndex(
                name: "IX_Faculities_InstituteId",
                table: "Faculity",
                newName: "IX_Faculity_InstituteId");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "DeviceAttendanceLogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAttendance",
                table: "StudentAttendance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FaculityAttendance",
                table: "FaculityAttendance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faculity",
                table: "Faculity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InstituteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    UserDeviceId = table.Column<long>(type: "bigint", nullable: false),
                    instituteId = table.Column<int>(type: "integer", nullable: false),
                    CandidateType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Faculity_Institutes_InstituteId",
                table: "Faculity",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FaculityAttendance_Faculity_FaculityId",
                table: "FaculityAttendance",
                column: "FaculityId",
                principalTable: "Faculity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FaculityAttendance_Institutes_InstituteId",
                table: "FaculityAttendance",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Courses_CourseId",
                table: "Student",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Institutes_InstituteId",
                table: "Student",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendance_Courses_CourseId",
                table: "StudentAttendance",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendance_Institutes_InstituteId",
                table: "StudentAttendance",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendance_Student_StudentId",
                table: "StudentAttendance",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculity_Institutes_InstituteId",
                table: "Faculity");

            migrationBuilder.DropForeignKey(
                name: "FK_FaculityAttendance_Faculity_FaculityId",
                table: "FaculityAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_FaculityAttendance_Institutes_InstituteId",
                table: "FaculityAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Courses_CourseId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Institutes_InstituteId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendance_Courses_CourseId",
                table: "StudentAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendance_Institutes_InstituteId",
                table: "StudentAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendance_Student_StudentId",
                table: "StudentAttendance");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAttendance",
                table: "StudentAttendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FaculityAttendance",
                table: "FaculityAttendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Faculity",
                table: "Faculity");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "DeviceAttendanceLogs");

            migrationBuilder.RenameTable(
                name: "StudentAttendance",
                newName: "StudentAttendances");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "FaculityAttendance",
                newName: "FaculityAttendances");

            migrationBuilder.RenameTable(
                name: "Faculity",
                newName: "Faculities");

            migrationBuilder.RenameColumn(
                name: "DeviceUserId",
                table: "DeviceAttendanceLogs",
                newName: "BiomerticUserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendance_StudentId",
                table: "StudentAttendances",
                newName: "IX_StudentAttendances_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendance_InstituteId",
                table: "StudentAttendances",
                newName: "IX_StudentAttendances_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAttendance_CourseId",
                table: "StudentAttendances",
                newName: "IX_StudentAttendances_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_InstituteId",
                table: "Students",
                newName: "IX_Students_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_CourseId",
                table: "Students",
                newName: "IX_Students_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_FaculityAttendance_InstituteId",
                table: "FaculityAttendances",
                newName: "IX_FaculityAttendances_InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_FaculityAttendance_FaculityId",
                table: "FaculityAttendances",
                newName: "IX_FaculityAttendances_FaculityId");

            migrationBuilder.RenameIndex(
                name: "IX_Faculity_InstituteId",
                table: "Faculities",
                newName: "IX_Faculities_InstituteId");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "BiomatricDevices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FaculityAttendances",
                table: "FaculityAttendances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faculities",
                table: "Faculities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculities_Institutes_InstituteId",
                table: "Faculities",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FaculityAttendances_Faculities_FaculityId",
                table: "FaculityAttendances",
                column: "FaculityId",
                principalTable: "Faculities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FaculityAttendances_Institutes_InstituteId",
                table: "FaculityAttendances",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Courses_CourseId",
                table: "StudentAttendances",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Institutes_InstituteId",
                table: "StudentAttendances",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Institutes_InstituteId",
                table: "Students",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
