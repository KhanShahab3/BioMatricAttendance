namespace BioMatricAttendance.DTOsModel
{
    public class AttendanceReportSummaryDto
    {
        public int TotalInstitutes { get; set; }
        public int TotalFaculty { get; set; }
        public int FacultyPresent { get; set; }
        public decimal FacultyAttendancePercentage { get; set; }
        public int TotalStudents { get; set; }
        public int StudentsPresent { get; set; }
        public decimal StudentAttendancePercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
