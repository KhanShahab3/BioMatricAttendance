namespace BioMatricAttendance.DTOsModel
{
    public class SuperAdminDashboardDto
    {
        public int TotalInstitutes { get; set; }

        public int TotalFaculty { get; set; }
        public int TotalFacultyPresent { get; set; }
        public int TotalStudents { get; set; }
        public int MaleStudentCount { get; set; }
        public int FemaleStudentCount { get; set; }
        public int MaleFaculityCount { get; set; }
        public int FemaleFaculityCount { get; set; }
        public int TotalStudentsPresent { get; set; }
        public int TotalDevices { get; set; }
        public int TotalDevicesActive { get; set; }
        //public int TotalDevicesInactive { get; set; }
        public int TotalRegisteredCourses { get; set; }
        public int FacultyPresentPercentage { get; set; }
        public int FacultyAbsentPercentage { get; set; }
        public int StudentsPresentPercentage { get; set; }
        public int StudentsAbsentPercentage { get; set; }

        public List<InstituteDashboardRowDto> InstituteDashboardRows { get; set; }



    }
}
