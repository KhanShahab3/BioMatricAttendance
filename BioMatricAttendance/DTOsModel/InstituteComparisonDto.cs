namespace BioMatricAttendance.DTOsModel
{
    public class InstituteComparisonDto
    {


        public string InstituteName { get; set; }
        public string DistrictName { get; set; }

        public int FacultyPresent { get; set; }
        public int TotalFaculty { get; set; }
        public decimal FacultyAttendancePercentage { get; set; }

        public int StudentsPresent { get; set; }
        public int TotalStudents { get; set; }
        public decimal StudentAttendancePercentage { get; set; }

        public int ActiveDevices { get; set; }
        public int TotalDevices { get; set; }

        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
    }
}
