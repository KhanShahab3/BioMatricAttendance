namespace BioMatricAttendance.DTOsModel
{
    public class RegionDashboardSummaryDto
    {

        public int TotalInstitutes { get; set; }

        public int TotalFaculty { get; set; }
        public int FacultyPresent { get; set; }
        public int FacultyAbsent { get; set; }
        public decimal FacultyAttendancePercentage { get; set; }

        public int TotalStudents { get; set; }
        public int StudentsPresent { get; set; }
        public int StudentsAbsent { get; set; }
        public decimal StudentAttendancePercentage { get; set; }

        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int InactiveDevices { get; set; }

        public int StaffMaleCount { get; set; }
        public int StaffFemaleCount { get; set; }
        public int StudentMaleCount { get; set; }
        public int StudentFemaleCount { get; set; }
        public int TotalCourses { get; set; }


    }
}
