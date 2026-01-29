namespace BioMatricAttendance.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Duration { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int InstituteId { get; set; }

        public Institute Institute { get; set; }
        public List<CandidateCourses> CandidateCourses { get; set; }

    }

}
