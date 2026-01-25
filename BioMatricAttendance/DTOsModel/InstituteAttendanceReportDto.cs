namespace BioMatricAttendance.DTOsModel
{
    public class InstituteAttendanceReportDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public FacultyAttendanceSummaryDto FacultySummary { get; set; }
        public StudentAttendanceSummaryDto StudentSummary { get; set; }
        public List<CourseAttendanceDto> CourseWiseAttendance { get; set; }
    }
}
