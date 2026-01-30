namespace BioMatricAttendance.DTOsModel
{
    public class RegionalStudentAttendanceDto
    {
        public string InstituteName { get; set; }
        public string District { get; set; }

        public int TotalStudents { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }

        public decimal AttendancePercentage { get; set; }
        public string Status { get; set; }
    }
}
