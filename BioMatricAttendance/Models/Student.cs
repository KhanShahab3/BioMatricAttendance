namespace BioMatricAttendance.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
       public int BiometricId { get; set; }

        //list of courses Id 
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int InstituteId { get; set; }
        public Institute? Institute { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; }= false;

        public List<StudentAttendance> Attendances { get; set; }

        //deviceId use as a foreign key

    }
}

