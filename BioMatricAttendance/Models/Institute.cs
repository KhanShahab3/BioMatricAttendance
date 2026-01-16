namespace BioMatricAttendance.Models
{
    public class Institute
    {
        public int Id { get; set; }
        public string InstituteName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }= false;
        public int RegionId { get; set; }
        public Region? Region { get; set; }

        public List<Faculity> Faculities { get; set; }
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<BiomatricDevice> BiomatricDevices { get; set; }
        public List<FaculityAttendance> FaculityAttendances { get; set; }
        public List<StudentAttendance> StudentAttendances { get; set; }
    }
}
