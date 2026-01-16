namespace BioMatricAttendance.DTOsModel
{
    public class FaculityAttendanceReports
    {
        public string InstituteName { get; set; }
        public string RegionName { get; set; }
        public int TotalFaculity { get; set; }
        public int FacultyPresent { get; set; }
        public int FacultyAbsent { get; set; }
        public int AttendancePercentage { get; set; }
        public string Status { get; set; }

        }
}
