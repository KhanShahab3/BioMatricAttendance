namespace BioMatricAttendance.DTOsModel
{
    public class CourseAttendanceDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int TotalStudents { get; set; }
        public int PresentStudents { get; set; }
        public int AbsentStudents { get; set; }

        public decimal AttendancePercentage { get; set; }
    }
}
