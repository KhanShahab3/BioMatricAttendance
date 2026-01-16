namespace BioMatricAttendance.DTOsModel
{
    public class InstituteDashboardRowDto
    {
        public string InstituteName { get; set; }
        public string RegionName { get; set; }

        public int FacultyPresent { get; set; }
        public int FacultyAbsent { get; set; }

        public int StudentPresent { get; set; }
        public int StudentAbsent { get; set; }

        public int DevicesActive { get; set; }
        public int DevicesInactive { get; set; }

        public int RegisteredCourses { get; set; }
    }
}
