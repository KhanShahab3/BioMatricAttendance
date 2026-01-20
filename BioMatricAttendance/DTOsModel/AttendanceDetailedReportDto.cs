namespace BioMatricAttendance.DTOsModel
{
    public class AttendanceDetailedReportDto
    {
        public AttendanceReportSummaryDto Summary { get; set; }
        public List<InstituteAttendanceRowDto> FacultyReport { get; set; }
        public List<InstituteAttendanceRowDto> StudentReport { get; set; }
    }
}
