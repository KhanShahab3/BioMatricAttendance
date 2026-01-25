namespace BioMatricAttendance.DTOsModel
{
    public class StudentAttendanceSummaryDto
    {

        public int TotalStudents { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public decimal AttendanceRate { get; set; }
    }
}
