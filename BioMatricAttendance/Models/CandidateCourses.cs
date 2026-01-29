using System.ComponentModel.DataAnnotations;

namespace BioMatricAttendance.Models
{
    public class CandidateCourses
    {
        [Key]
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
