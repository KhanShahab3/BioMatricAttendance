namespace BioMatricAttendance.DTOsModel
{
    public class SuperAdminDashboardDto
    {
        public int TotalInstitutes { get; set; }

        public int TotalFacultyPresent { get; set; }
        public int TotalFacultyAbsent { get; set; }
        public int TotalStudentsPresent { get; set; }
        public int TotalStudentsAbsent { get; set; }
        public int TotalDevicesActive { get; set; }
        public int TotalDevicesInactive { get; set; }
        public int TotalRegisteredCourses { get; set; }
        public int FacultyPresentPercentage { get; set; }
        public int FacultyAbsentPercentage { get; set; }
        public int StudentsPresentPercentage { get; set; }
        public int StudentsAbsentPercentage { get; set; }



    }
}
