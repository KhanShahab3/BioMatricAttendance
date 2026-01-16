namespace BioMatricAttendance.Models
{
    public class Faculity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BiometricId { get; set; }
        public int InstituteId { get; set; }
        public Institute? Institute { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfJoining { get; set; } 
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }= false;

        public List<FaculityAttendance> Attendances { get; set; }
    }
    //deviceId use as a foreign key 
}


