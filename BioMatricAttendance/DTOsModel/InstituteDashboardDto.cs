using System.ComponentModel;

namespace BioMatricAttendance.DTOsModel
{
    public class InstituteDashboardDto
    {

        public int InstituteId { get; set; }
        public string InstituteName { get; set; }  =string.Empty;
        public string Address { get; set; } = string.Empty;
        public int FacultyPresent { get; set; }
        public int FacultyMale { get; set; }
        public int FacultyFemale { get; set; }
        public int StudentMale { get; set; }
        public int StudentFemale { get; set; }
        public int TotalFaculty { get; set; }
        public int FacultyAbsent { get; set; }
        public int ActiveDevices { get; set; }
        public int InActiveDevices { get; set; } 

       
        //public int StaffOvertime { get; set; }

        public int TotalStudents { get; set; }
        public int StudentsPresent { get; set; }

        public int TotalCourses { get; set; }
        public int StudentsAbsent { get; set; }

        public List<ShiftWiseAttendanceDto> ShiftWiseAttendance { get; set; }

        //public int StudentsOvertime { get; set; }

        //public List<ShiftAttendanceDto> ShiftWise { get; set; }
        //public List<CourseAttendanceDto> CourseWise { get; set; }
    }
}
