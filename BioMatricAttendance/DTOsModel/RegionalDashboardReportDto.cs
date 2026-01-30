namespace BioMatricAttendance.DTOsModel
{
    public class RegionalDashboardReportDto
    {

        public int TotalInstitutes { get; set; }

        public int TotalFaculty { get; set; }
        public int FacultyPresent { get; set; }
        public decimal FacultyAttendancePercentage { get; set; }

        public int TotalStudents { get; set; }
        public int StudentsPresent { get; set; }
        public decimal StudentAttendancePercentage { get; set; }

       
        public List<RegionalFaculityAttendanceDto> FacultyAttendanceReport { get; set; }
        public List<RegionalStudentAttendanceDto> StudentAttendanceReport { get; set; }
    }
}
