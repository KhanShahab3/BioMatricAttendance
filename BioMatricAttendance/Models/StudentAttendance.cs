namespace BioMatricAttendance.Models
{
    public class StudentAttendance
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Status { get; set; }
        public string ? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int InstituteId { get; set; }
        public Institute Institute { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}

