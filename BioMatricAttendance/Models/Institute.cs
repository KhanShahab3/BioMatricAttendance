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
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<BiomatricDevice> BiomatricDevices { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}

//get institue and region name courese counts 
//when click i want how many course in that institue having courase code course name and duration
