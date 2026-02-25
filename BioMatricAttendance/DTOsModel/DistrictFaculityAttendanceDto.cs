namespace BioMatricAttendance.DTOsModel
{
    public class DistrictFaculityAttendanceDto
    {
        public string InstituteName { get; set; }
        public string District { get; set; }

        public int TotalFaculty { get; set; }
        public int Present { get; set; }
        public int Absent => TotalFaculty - Present;

        public decimal AttendancePercentage { get; set; }
        public string Status { get; set; }
    }
}
